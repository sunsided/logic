using System;
using Logic.LanguageParser;
using Logic.LanguageParser.Descriptions;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            TokenDescriptionSet tokenDescriptions = new TokenDescriptionSet();
            tokenDescriptions.Add(new OperatorToken(@"^[\+\*]$", "Operatoren (binär)"));
            tokenDescriptions.Add(new PostfixNegationToken(@"^'$", "Negation (postfix)"));
            tokenDescriptions.Add(new PrefixNegationToken(@"^~$", "Negation (prefix)"));
            tokenDescriptions.Add(new TermToken(@"^[a-zA-Z]+([0-9]|[a-zA-Z]|_)*$", "Bezeichner"));
            tokenDescriptions.Add(new GroupOpenToken(@"^\($", "Klammern (öffnend)"));
            tokenDescriptions.Add(new GroupCloseToken(@"^\)$", "Klammern (schließend)"));

		    string equation = "foo + (alpha + beta') * (input3 + ~data_avail)";

            // Token basicToken = new Token(equation);
		    var tokenList = tokenDescriptions.Parse(equation);
		    
            Console.WriteLine(":P");
            Console.ReadKey();
		}
	}
}
