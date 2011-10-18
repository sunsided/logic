using System;
using Logic.LanguageParser;
using Logic.LanguageParser.Descriptions;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
		    const string binaryOperatorTerms = "and|or|nand|nor|xor";
            const string unaryOperatorTerms = "not";

            Parser tokenDescriptions = new Parser();
            tokenDescriptions.Add(new BinaryOperatorToken(@"^([\+\*\|\^]|" + binaryOperatorTerms + ")", "Operatoren (binär)"));
            tokenDescriptions.Add(new PostfixNegationToken(@"^'", "Negation (postfix)"));
            tokenDescriptions.Add(new PrefixNegationToken(@"^(~|" + unaryOperatorTerms + ")", "Negation (prefix)"));
            tokenDescriptions.Add(new TermToken(@"^(?!(" + binaryOperatorTerms + "|" + unaryOperatorTerms + "))[a-z]+([0-9]|[a-z]|_)*", "Term"));
            tokenDescriptions.Add(new GroupOpenToken(@"^\(", "Klammern (öffnend)"));
            tokenDescriptions.Add(new GroupCloseToken(@"^\)", "Klammern (schließend)"));

		    string equation = "foo + (Alpha + Beta') * (input3 + ~data_avail) and not foo";

            // Token basicToken = new Token(equation);
		    var tokenList = tokenDescriptions.Parse(equation);

            // Ausgeben, weil wegen
            Console.WriteLine("{0,-5}{1,-20}{2}", "Idx", "Wert", "Klasse");
            Console.WriteLine();
            for (int i=0; i<tokenList.Count; ++i)
            {
                Token token = tokenList[i];
                Console.WriteLine("{0,-5}{1,-20}{2}" , i, token.Value, token.OriginDescription.Description);
            }
            
            // Abbruch.
            Console.WriteLine();
            Console.WriteLine("Teste zum Beenden ...");
            Console.ReadKey(true);
		}
	}
}
