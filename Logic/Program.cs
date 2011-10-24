using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Logic.Nodes;
using Logic.Sequence;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            Parser parser = new Parser();
		    parser.AddDescription("AND", TokenType.And).AddKeyword("and", "*", "&", "&&");
            parser.AddDescription("OR", TokenType.Or).AddKeyword("or", "+", "^", "|", "||");
            parser.AddDescription("XOR", TokenType.Xor).AddKeyword("xor");
            parser.AddDescription("NOR", TokenType.Nor).AddKeyword("nor");
            parser.AddDescription("XNOR", TokenType.Xnor).AddKeyword("xnor");
            parser.AddDescription("NAND", TokenType.Nand).AddKeyword("nand");
            parser.AddDescription("NOT>", TokenType.Not).AddKeyword("not", "!", "~");
            parser.AddDescription("<NOT", TokenType.NotReverse).AddKeyword("'");
            parser.AddDescription("GRPS", TokenType.GroupStart).AddKeyword("(");
            parser.AddDescription("GRPE", TokenType.GroupEnd).AddKeyword(")");
            parser.AddDescription("TERM", TokenType.Term).AddGenericTerms().IgnoreWord("and", "nand", "or", "nor", "xnor", "xor", "not");
            
		    // const string equation = "(a1 and !(b' + c)) | (a1 nand a2)' + d*c";
            const string equation = "(a * b + c * d)' or (e + f and g) and not h";
            // const string equation = "(a * b) or (c * d)";

            //                             
            //             OR              
            //           /    \            
            //         /        \          
            //      OR           AND       
            //     /  \         /   \      
            //  AND    AND     h    OR      <--|
            //   /\     /\          /\         | Zusammenfassen zu n-ärem Operator?
            //  a  b   c  d        e  OR    <--|
            //                        /\   
            //                       f  g  
            //                             

            IList<TokenMatch> result = parser.Parse(equation);
            IList<TokenSequenceEntry> sequence = MatchListToHierarchySequence(parser, result);
		    sequence = ReoderAnds(sequence);

            Queue<TokenSequenceEntry> seqQueue = new Queue<TokenSequenceEntry>(sequence); 
            DumpSequence(sequence, 0);
            
            // Hierarchiebaum erzeugen
            TokenNode node = CreateHierarchyTree(seqQueue);

            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Erzeugt eine Queue aus der Sequenz und ordnet die ANDs neu an
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private static IList<TokenSequenceEntry> ReoderAnds(IList<TokenSequenceEntry> sequence)
        {
            Contract.Requires(sequence != null);
            Contract.Ensures(Contract.Result<IList<TokenSequenceEntry>>() != null);

            // Die neue Liste
            IList<TokenSequenceEntry> tokenSequenceEntries = new List<TokenSequenceEntry>();

            // Die Sammelliste
            IList<TokenSequenceEntry> workList = new List<TokenSequenceEntry>();
            for (int s=0; s<sequence.Count; ++s)
            {
                TokenSequenceEntry entry = sequence[s];

                // Handelt es sich um ein Token, muss zwischen ODER-Operation
                // und regulärem Token unterschieden werden.
                if (entry is TokenEntry)
                {
                    if (((TokenEntry)entry).IsOrOperation())
                    {
                        // Wenn in der Arbeitsliste mehr als ein Eintrag ist, Untersequenz erzeugen
                        Contract.Assume(workList.Count > 0);
                        if (workList.Count > 1)
                        {
                            // Gespeicherte Sequenz eintragen
                            SequenceEntry left = new SequenceEntry(workList);
                            tokenSequenceEntries.Add(left);
                        }
                        else
                        {
                            // Ansonsten diesen Eintrag einzeln hinzufügen
                            tokenSequenceEntries.Add(workList[0]);
                        }

                        // Arbeitsliste leeren
                        workList = new List<TokenSequenceEntry>();
                        
                        // ODER eintüten
                        tokenSequenceEntries.Add(entry);
                    }
                    else
                    {
                        // Wenn es ein reguläres Token ist, dieses in die Verarbeitungsliste packen
                        workList.Add(entry);
                    }
                }
                else
                {
                    // Handelt es sich um eine Untersequenz, diese rekursiv behandeln und
                    // das Ergebnis als Sequenz-Entry in die Verarbeitungsliste packen.
                    Contract.Assume(entry is SequenceEntry);

                    // Bionic on!
                    SequenceEntry sequenceEntry = (SequenceEntry) entry;
                    IList<TokenSequenceEntry> list = ReoderAnds(sequenceEntry.ChildSequence);

                    // Nur bei mehreren Einträgen Untersequenz erzeugen
                    if (list.Count > 1)
                    {
                        workList.Add(new SequenceEntry(list));
                    }
                    else if (list.Count == 1)
                    {
                        workList.Add(list[0]);
                    }
                }
            }

            // Am Ende der Verarbeitung muss der in der Liste verbliebene Rest noch 
            // zur Ausgabeliste hinzugefügt werden.
            if (workList.Count > 1)
            {
                // Aus Sequenzen Sequenz-Einträge machen
                SequenceEntry rest = new SequenceEntry(workList);
                tokenSequenceEntries.Add(rest);
            }
            else if (workList.Count == 1)
            {
                // Einzelne Elemente dürfen einzeln angehängt werden.
                tokenSequenceEntries.Add(workList[0]);
            }

            // Und tschüss!
            return tokenSequenceEntries;
        }
        
        /// <summary>
        /// Extrahiert Knoten aus der Sequenz
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private static TokenNode ExtractNode(Queue<TokenSequenceEntry> sequence)
        {
            Contract.Requires(sequence != null);
            Contract.Ensures(Contract.Result<TokenNode>() != null);

            TokenSequenceEntry tse = sequence.Dequeue();
            Contract.Assume(tse != null);

            if (tse is SequenceEntry)
            {
                SequenceEntry se = (SequenceEntry) tse;
                return CreateHierarchyTree(new Queue<TokenSequenceEntry>(se.ChildSequence));
            }

            TokenEntry te = tse as TokenEntry;
            Contract.Assume(te != null);

            switch(te.Match.Type)
            {
                case TokenType.Not:
                    UnaryOperatorNode notOperator = new UnaryOperatorNode {Match = te.Match, Node = ExtractNode(sequence)};
                    return notOperator;

                case TokenType.Term:
                    TermNode termNode = new TermNode {Match = te.Match};
                    return termNode;

                case TokenType.And:
                case TokenType.Nand:
                case TokenType.Or:
                case TokenType.Nor:
                case TokenType.Xor:
                case TokenType.Xnor:
                    BinaryOperatorNode binaryNode = new BinaryOperatorNode();
                    binaryNode.Match = te.Match;
                    return binaryNode;
            }

            return null;
        }

        /// <summary>
        /// Creates the hierarchy tree.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <remarks></remarks>
        private static TokenNode CreateHierarchyTree(Queue<TokenSequenceEntry> sequence)
        {
            Contract.Requires(sequence != null, "Sequenz darf nicht null sein");
            Contract.Ensures(Contract.Result<TokenNode>() != null);

            Queue<TokenNode> nodeStack = new Queue<TokenNode>();

            while (sequence.Count > 0)
            {
                // Token lesen und merken
                TokenNode node = ExtractNode(sequence);
                nodeStack.Enqueue(node);

                // Wenn drei Token gemerkt sind
                if (nodeStack.Count == 3)
                {
                    // Alle Elemente entnehmen
                    TokenNode left = nodeStack.Dequeue();
                    BinaryOperatorNode binaryNode = (BinaryOperatorNode)nodeStack.Dequeue();
                    TokenNode right = nodeStack.Dequeue();

                    // Nachtragen
                    binaryNode.LeftNode = left;
                    binaryNode.RightNode = right;

                    // Wieder eintüten
                    nodeStack.Enqueue(binaryNode);
                }

                // TODO: Nächsten Token verarbeiten
                // TODO: Hauptverarbeitungslogik
            }
            return nodeStack.Dequeue();
        }

	    #region Dump

        /// <summary>
        /// Gibt einen einfachen Dump der Sequenz aus
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="indentLevel"></param>
        private static void DumpSequence(IList<TokenSequenceEntry> sequence, int indentLevel)
        {
            const int indentDepth = 2;
            for (int s = 0; s < sequence.Count; ++s)
            {
                TokenSequenceEntry entry = sequence[s];
                if (entry is SequenceEntry)
                {
                    Console.WriteLine(new String(' ', indentLevel * indentDepth) + "{");                   
                    DumpSequence(((SequenceEntry)entry).ChildSequence, indentLevel + 1);
                    Console.WriteLine(new String(' ', indentLevel * indentDepth) + "}");
                }
                else
                {
                    Console.Write(new String(' ', indentLevel * indentDepth));

                    TokenMatch match = ((TokenEntry) entry).Match;
                    if (match.Type == TokenType.Term)
                    {
                        Console.WriteLine(match.Type + "(" + match.Token + ")");
                    }
                    else
                    {
                        Console.WriteLine(match.Type);
                    }
                }
            }
        }

        #endregion Dump

        #region Sequenzen

        /// <summary>
        /// Wandelt die Liste in eine hierarchische Sequenz um
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IList<TokenSequenceEntry> MatchListToHierarchySequence(Parser parser, IList<TokenMatch> list)
        {
            Contract.Requires(parser != null);
            Contract.Requires(list != null);
            Contract.Ensures(Contract.Result<IList<TokenSequenceEntry>>() != null);

            // Die aktuelle Sequenz
            IList<TokenSequenceEntry> sequence = new List<TokenSequenceEntry>();

            // Der Stack der Sequenzen für die Gruppierungen
            Stack<IList<TokenSequenceEntry>> groupStack = new Stack<IList<TokenSequenceEntry>>();

            // Alle Token durchlaufen
            for (int i=0; i<list.Count; ++i)
            {
                TokenMatch match = list[i];

                // Gruppierungen überprüfen
                if (match.Type == TokenType.GroupStart)
                {
                    // Den aktuellen Eintrag (group start) ignorieren.
                    // Aktuelle Gruppe auf den Stack legen und neue Sequenz beginnen.
                    groupStack.Push(sequence);
                    sequence = new List<TokenSequenceEntry>();
                }
                else if(match.Type == TokenType.GroupEnd)
                {
                    // Sanity check:
                    if (groupStack.Count == 0) throw new ParenthesesMismatchException(ParenthesesMismatchException.ErrorType.TooManyClosing, "Syntaxfehler: Zu viele schließende Klammern.");

                    // Den aktuellen Eintrag (group end) ignorieren.
                    // Sequenzeintrag erzeugen, Elternsequenz vom Stack holen und Eintrag eintüten
                    TokenSequenceEntry entry = new SequenceEntry(sequence);
                    sequence = groupStack.Pop();
                    sequence.Add(entry);
                }
                else
                {
                    // Aktuellen Eintrag als TokenEntry eintüten
                    TokenEntry entry = new TokenEntry(match);
                    sequence.Add(entry);
                }
            }

            // Sanity check
            if (groupStack.Count != 0) throw new ParenthesesMismatchException(ParenthesesMismatchException.ErrorType.TooFewClosing, "Syntaxfehler: Schließende Klammer fehlt.");

            // Reverse-NOT entfernen
            ReverseNotToForwardNot(parser, ref sequence);
            return sequence;
        }

        /// <summary>
        /// Entfernt Reverse-NOT und ersetzt sie durch Forward-NOT
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="sequence">Die zu beackernde Sequenz</param>
        /// <remarks></remarks>
        private static void ReverseNotToForwardNot(Parser parser, ref IList<TokenSequenceEntry> sequence)
        {
            Contract.Requires(parser != null);
            Contract.Requires(sequence != null);
            Contract.Ensures(Contract.ValueAtReturn(out sequence) != null);

            // Sequenz durchlaufen
            for (int s=0; s<sequence.Count; ++s)
            {
                TokenSequenceEntry entry = sequence[s];

                // Als Untersequenz auswerten
                if (entry is SequenceEntry)
                {
                    IList<TokenSequenceEntry> subSequence = ((SequenceEntry) entry).ChildSequence;
                    ReverseNotToForwardNot(parser, ref subSequence);
                    continue;
                }

                // Als Token auswerten
                TokenEntry token = entry as TokenEntry;
                Contract.Assume(token != null);
                if (token.Match.Type == TokenType.NotReverse)
                {
                    TokenMatch currentMatch = token.Match;

                    // Neue Beschreibung erzwingen
                    TokenDescription notDescription = parser.GetDescription(TokenType.Not);
                    ITokenDescription newDescription = currentMatch.Description.GetOverride(notDescription);

                    // Neues Token generieren
                    TokenMatch newMatch = new TokenMatch(currentMatch.Index, currentMatch.Token, newDescription);

                    // Neues Token einsetzen
                    sequence.Insert(s-1, new TokenEntry(newMatch));

                    // Aktuelles Token entfernen.
                    // Normalerweise müsste (s) entfernt werden - da jedoch durch die Insertoperation
                    // die Indizes um eines verschoben sind, gilt (s+1). Anschließend
                    // den Index von s um eines reduzieren, damit die Schleifenoperation nicht fehlschlägt.
                    sequence.RemoveAt((s--)+1);
                }
            }
        }

        #endregion Sequenzen
    }
}
