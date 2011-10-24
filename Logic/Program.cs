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
            
            const string equation = "(a * b + c * d)' or (e + f and g) and not h";

            // Parsen
            IList<TokenSequenceEntry> result = parser.Parse(equation);

            // Hierarchiebaum erzeugen
            TreeGenerator generator = new TreeGenerator();
            TokenNode node = generator.GenerateHierarchy(result);
            
            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
        }
    }
}
