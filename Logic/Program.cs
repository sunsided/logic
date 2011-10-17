using System;
using Logic.LanguageParser;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            TokenDescriptionSet tokenDescriptions = new TokenDescriptionSet();
            tokenDescriptions.Add(new TokenDescription(@"^[\+\*]$", "Operatoren (binär)"));
            tokenDescriptions.Add(new TokenDescription(@"^'$", "Negation (postfix)"));
            tokenDescriptions.Add(new TokenDescription(@"^~$", "Negation (prefix)"));
            tokenDescriptions.Add(new TokenDescription(@"^[a-zA-Z]+([0-9]|[a-zA-Z]|_)*$", "Bezeichner"));
            tokenDescriptions.Add(new TokenDescription(@"^\($", "Klammern (öffnend)"));
            tokenDescriptions.Add(new TokenDescription(@"^\)$", "Klammern (schließend)"));

		    string equation = "foo + (alpha + beta') * (input3 + ~data_avail)";

            // Token basicToken = new Token(equation);
		    var tokenList = tokenDescriptions.Parse(equation);
		    
            Console.WriteLine(":P");
            Console.ReadKey();
		}
	}
}
