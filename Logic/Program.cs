using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
            const string equation = "(a * b + c * d) + (e + f + g) * h";

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
		    DumpSequence(sequence, 0);
            
            // Hierarchiebaum erzeugen
		    CreateHierarchyTree(sequence);

            // Abbruch.
		    Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
		}

        /// <summary>
        /// Creates the hierarchy tree.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <remarks></remarks>
        private static void CreateHierarchyTree(IList<TokenSequenceEntry> sequence)
        {
            Contract.Requires(sequence != null, "Sequenz darf nicht null sein");

            // Liste an OR-Operatoren trennen
            IList<IList<TokenSequenceEntry>> sequences = SplitSequences(sequence, TokenType.Or, TokenType.Nor, TokenType.Xor, TokenType.Xnor);

            // TODO: Wenn mehr als zwei Elemente: Diese sind ODER-verknüpft!
        }

        /// <summary>
        /// Trennt eine Liste bei bestimmten Token
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="splitAt"></param>
        /// <param name="splitAtAdditional"></param>
        /// <returns></returns>
        private static IList<IList<TokenSequenceEntry>> SplitSequences(IList<TokenSequenceEntry> sequence, IList<TokenType> splitAtList)
        {
            Contract.Requires(sequence != null, "Sequenz darf nicht null sein");
            Contract.Requires(splitAtList != null, "Parameter dürfen nicht null sein");
            Contract.Ensures(Contract.Result<IList<IList<TokenSequenceEntry>>>() != null);

            // Sequenz in ODER-Gruppen trennen
            IList<IList<TokenSequenceEntry>> subSequences = new List<IList<TokenSequenceEntry>>();
            int lastSplitIndex = 0;
            for (int s = 0; s < sequence.Count; ++s)
            {
                TokenEntry token = sequence[s] as TokenEntry;
                if (token == null) continue;

                // Prüfen, ob an dem Typen getrennt werden soll
                if (!splitAtList.Contains(token.Match.Type)) continue;

                // Elemente extrahieren
                subSequences.Add(ExtractRange(sequence, lastSplitIndex, s));

                // Neuen Index merken
                lastSplitIndex = s + 1; // Das ODER überspringen
                break;
            }

            // Fehlende Elemente eintüten
            subSequences.Add(ExtractRange(sequence, lastSplitIndex, sequence.Count));
            return subSequences;
        }

	    /// <summary>
        /// Trennt eine Liste bei bestimmten Token
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="splitAt"></param>
        /// <param name="splitAtAdditional"></param>
        /// <returns></returns>
        private static IList<IList<TokenSequenceEntry>> SplitSequences(IList<TokenSequenceEntry> sequence, TokenType splitAt, params TokenType[] splitAtAdditional)
	    {
            Contract.Requires(sequence != null, "Sequenz darf nicht null sein");
            Contract.Requires(splitAtAdditional != null, "Parameter dürfen nicht null sein");
            Contract.Ensures(Contract.Result<IList<IList<TokenSequenceEntry>>>() != null);

            // Splitliste erzeugen
            List<TokenType> splitAtList = new List<TokenType>(splitAtAdditional);
            splitAtList.Add(splitAt);

            // Und weiterreichen
	        return SplitSequences(sequence, splitAtList);
	    }

	    /// <summary>
        /// Extrahiert Elemente aus einer Sequenz
        /// </summary>
        /// <param name="sequence">Die Sequenz</param>
        /// <param name="toExclusive">Der Index, vor dem gestoppt werden soll</param>
        /// <param name="from">Der Startindex</param>
        /// <returns>Die extrahierte Liste</returns>
	    private static IList<TokenSequenceEntry> ExtractRange(IList<TokenSequenceEntry> sequence, int from, int toExclusive)
	    {
            Contract.Requires(sequence != null, "Sequenz darf nicht null sein");
            Contract.Ensures(Contract.Result<IList<TokenSequenceEntry>>() != null);

	        IList<TokenSequenceEntry> subSequence = new List<TokenSequenceEntry>();
	        for (int x = from; x < toExclusive; ++x)
	        {
	            subSequence.Add(sequence[x]);
	        }
	        return subSequence;
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
