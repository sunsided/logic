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
            
		    const string equation = "(a1 and !(b + c)) | (a1 nand a2)'";

            IList<TokenMatch> result = parser.Parse(equation);
		    IList<TokenSequenceEntry> sequence = MatchListToHierarchySequence(result);

            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
		}

        /// <summary>
        /// Wandelt die Liste in eine hierarchische Sequenz um
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IList<TokenSequenceEntry> MatchListToHierarchySequence(IList<TokenMatch> list)
        {
            Contract.Requires(list != null);

            // Die aktuelle Sequenz
            List<TokenSequenceEntry> sequence = new List<TokenSequenceEntry>();

            // Der Stack der Sequenzen für die Gruppierungen
            Stack<List<TokenSequenceEntry>> groupStack = new Stack<List<TokenSequenceEntry>>();

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
                    if (groupStack.Count == 0) throw new ArgumentException("list", "Syntaxfehler: Zu viele schließende Klammern.");

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

            // Und raus.
            if (groupStack.Count != 0) throw new ArgumentException("list", "Syntaxfehler: Schließende Klammer fehlt.");
            return sequence;
        }
	}
}
