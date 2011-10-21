using System;
using System.Linq;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            TokenDescription and = new TokenDescription("AND");
            and.AddKeyword("and", "*", "&", "&&");

            TokenDescription or = new TokenDescription("OR");
            and.AddKeyword("or", "+", "^", "|", "||");

		    string sequence = "  and now what?";
		    TokenMatch match;
            if (and.IsMatch(sequence, 0, out match))
            {
                Console.Write(sequence);
            }

            sequence = "or now what?";
            if (and.IsMatch(sequence, 0, out match))
            {
                Console.Write(sequence);
            }

            /*
		    const string andOperatorTerms = "and";
            const string orOperatorTerms = "or";
            const string norOperatorTerms = "or";
            const string xorOperatorTerms = "or";
            const string xnorOperatorTerms = "or";
            const string nandOperatorTerms = "or";
		    string binaryOperatorTerms = andOperatorTerms + "|" + orOperatorTerms + "|" + norOperatorTerms + "|" +
		                                 xorOperatorTerms + "|" + xnorOperatorTerms + "|" + nandOperatorTerms;
            const string unaryOperatorTerms = "not";

            Parser parser = new Parser();
            parser.Add(new AndOperatorToken(@"^([\*&]|" + andOperatorTerms + ")", "AND"));
            parser.Add(new AndOperatorToken(@"^(" + nandOperatorTerms + ")", "NAND"));
            parser.Add(new OrOperatorToken(@"^([\+\|\^]|" + orOperatorTerms + ")", "OR"));
            parser.Add(new NorOperatorToken(@"^(" + norOperatorTerms + ")", "NOR"));
            parser.Add(new XorOperatorToken(@"^(" + xorOperatorTerms + ")", "XOR"));
            parser.Add(new XnorOperatorToken(@"^(" + xnorOperatorTerms + ")", "XNOR"));
            parser.Add(new PostfixNegationToken(@"^'", "Negation (postfix)"));
            parser.Add(new PrefixNegationToken(@"^(~|!|" + unaryOperatorTerms + ")", "Negation (prefix)"));
            parser.Add(new TermToken(@"^(?!(" + binaryOperatorTerms + "|" + unaryOperatorTerms + "))[a-z]+([0-9]|[a-z]|_)*", "Term"));
            parser.Add(new GroupOpenToken(@"^\(", "Klammern (öffnend)"));
            parser.Add(new GroupCloseToken(@"^\)", "Klammern (schließend)"));

		    const string equation = "foo + (Alpha + Beta') * (input3 + ~data_avail) and not foo";

            // Token basicToken = new Token(equation);
		    IList<Token> tokenList = parser.Parse(equation);

            // Ausgeben, weil wegen
            Console.WriteLine("{0,-5}{1,-20}{2}", "Idx", "Wert", "Klasse");
            Console.WriteLine();
            for (int ti=0; ti<tokenList.Count; ++ti)
            {
                Token token = tokenList[ti];
                Console.WriteLine("{0,-5}{1,-20}{2}" , token.Index, token.Value, token.OriginDescription.Description);
            }
            
            // Intepretation durchführen
            Interpreter interpreter = new Interpreter();
            interpreter.Interpret(tokenList);
            */

            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
		}
	}
}
