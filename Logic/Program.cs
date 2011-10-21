using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            Parser parser = new Parser();
		    parser.AddDescription("AND").AddKeyword("and", "*", "&", "&&");
            parser.AddDescription("OR").AddKeyword("or", "+", "^", "|", "||");
            parser.AddDescription("XOR").AddKeyword("xor");
            parser.AddDescription("NOR").AddKeyword("nor");
            parser.AddDescription("XNOR").AddKeyword("xnor");
            parser.AddDescription("NAND").AddKeyword("nand");
            parser.AddDescription("NOT>").AddKeyword("not", "!", "~");
            parser.AddDescription("<NOT").AddKeyword("'");
            parser.AddDescription("GRPS").AddKeyword("(");
            parser.AddDescription("GRPE").AddKeyword(")");
		    parser.AddDescription("TERM").AddGenericTerms().IgnoreWord("and", "nand", "or", "nor", "xnor", "xor", "not");
            
		    string sequence = "(a1 and b) | (a1 nand a2)'";
		    
		    IList<TokenMatch> result = parser.Parse(sequence);

            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
		}
	}
}
