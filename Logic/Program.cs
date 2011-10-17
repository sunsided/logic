using System.Collections.Generic;
using Logic.LanguageParser;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            TokenDescriptionSet tokenDescriptions = new TokenDescriptionSet();
            tokenDescriptions.Add(new TokenDescription(@"[\+\*]", "Operatoren (binär)"));
            tokenDescriptions.Add(new TokenDescription(@"[~']", "Operatoren (unär)"));
            tokenDescriptions.Add(new TokenDescription(@"[a-zA-Z]+([0-9]*[a-zA-Z]*_*])*", "Bezeichner"));
            tokenDescriptions.Add(new TokenDescription(@"[()]", "Klammern"));


		}
	}
}
