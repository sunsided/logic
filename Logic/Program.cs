using System;
using System.Collections.Generic;
using Logic.LanguageParser;
using Logic.LanguageParser.Descriptions;
using Logic.TokenInterpreter;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
		    const string binaryOperatorTerms = "and|or|nand|nor|xor";
            const string unaryOperatorTerms = "not";

            Parser parser = new Parser();
            parser.Add(new BinaryOperatorToken(@"^([\+\*\|\^]|" + binaryOperatorTerms + ")", "Operatoren (binär)"));
            parser.Add(new PostfixNegationToken(@"^'", "Negation (postfix)"));
            parser.Add(new PrefixNegationToken(@"^(~|" + unaryOperatorTerms + ")", "Negation (prefix)"));
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

            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Taste zum Beenden ...");
            Console.ReadKey(true);
		}
	}
}
