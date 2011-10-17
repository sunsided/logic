using System;
using Logic.LanguageParser;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            TokenDescriptionSet tokenDescriptions = new TokenDescriptionSet();
            tokenDescriptions.Add(new TokenDescription(@"[\+\*]", "Operatoren (binär)"));
            tokenDescriptions.Add(new TokenDescription(@"[~']", "Negation"));
            tokenDescriptions.Add(new TokenDescription(@"[a-zA-Z]+([0-9]|[a-zA-Z]|_])*", "Bezeichner"));
            tokenDescriptions.Add(new TokenDescription(@"\(", "Klammern (öffnend)"));
            tokenDescriptions.Add(new TokenDescription(@"\)", "Klammern (schließend)"));

		    string equation = "(alpha + beta) * (input + data_avail')";

            // Token basicToken = new Token(equation);
		    tokenDescriptions.Parse(equation);
		    Console.ReadKey();
		}
	}
}
