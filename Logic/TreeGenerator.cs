using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Logic.Nodes;
using Logic.Nodes.UnaryOperators;
using Logic.Sequence;

namespace Logic
{
    /// <summary>
    /// Erzeugt den Baum
    /// </summary>
    public sealed class TreeGenerator
    {
        /// <summary>
        /// Erzeugt eine Hierarchie aus der Tokensequenz
        /// </summary>
        /// <param name="sequence">Die Sequenz</param>
        /// <returns>Die Hierarchie</returns>
        public TokenTree GenerateHierarchy(IList<TokenSequenceEntry> sequence)
        {
            Contract.Requires(sequence != null, "Sequenz darf nicht null sein");
            Contract.Ensures(Contract.Result<TokenTree>() != null);

            // ANDs gruppieren
            sequence = ReoderAnds(sequence);

            // Eigentliche Erzeugungslogik
            TokenNode node = CreateHierarchyTree(new Queue<TokenSequenceEntry>(sequence));

            // Tree erzeugen
            TokenTree tree = new TokenTree(node);
            return tree;
        }
        
        /// <summary>
        /// Creates the hierarchy tree.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <remarks></remarks>
        private TokenNode CreateHierarchyTree(Queue<TokenSequenceEntry> sequence)
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

        /// <summary>
        /// Entnimmt einen Eintrag aus der Sequenz und erzeugt einen zugehörigen Node
        /// </summary>
        /// <param name="sequence">Die Sequenz</param>
        /// <returns>Der Node</returns>
        private TokenNode ExtractNode(Queue<TokenSequenceEntry> sequence)
        {
            Contract.Requires(sequence != null);
            Contract.Ensures(Contract.Result<TokenNode>() != null);

            TokenSequenceEntry tse = sequence.Dequeue();
            Contract.Assume(tse != null);

            // Wenn es sich um einen Sequenzeintrag handelt - Rekursiv verarbeiten
            if (tse is SequenceEntry)
            {
                SequenceEntry se = (SequenceEntry)tse;
                return CreateHierarchyTree(new Queue<TokenSequenceEntry>(se.ChildSequence));
            }

            // Annehmen, dass es sich ansonsten um ein Token handelt
            TokenEntry te = tse as TokenEntry;
            Contract.Assume(te != null);

            // Wenn es sich um eine Negation handelt, einen
            // weiteren Knoten lesen und diesen als Unterelement eines unären Operators speichern
            if (te.IsNotOperation())
            {
                UnaryOperatorNode notOperator = new NotNode { Match = te.Match, Node = ExtractNode(sequence) };
                return notOperator;
            }

            // Wenn es sich um eine binäre Operation handelt,
            // unären Operator erzeugen
            if (te.IsBinaryOperation())
            {
                BinaryOperatorNode binaryNode = (BinaryOperatorNode) TokenTypeNodeCache.CreateNode(te.Match.Type);
                binaryNode.Match = te.Match;
                return binaryNode;
            }

            // Wenn es sich um einen Term handelt ...
            if (te.IsTerm())
            {
                TermNode termNode = new TermNode {Match = te.Match};
                return termNode;
            }

            return null;
        }

        #region Umsortieren von ANDs

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
            for (int s = 0; s < sequence.Count; ++s)
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
                    SequenceEntry sequenceEntry = (SequenceEntry)entry;
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

        #endregion Umsortieren von ANDs
    }
}
