using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.LanguageParser;

namespace Logic
{
	class Program
	{
		static void Main(string[] args)
		{
            IList<TokenDescription> tokenDescriptions = new List<TokenDescription>();
            tokenDescriptions.Add(new TokenDescription(@"[\+\*]", "Operatoren (binär)"));
            tokenDescriptions.Add(new TokenDescription(@"[~']", "Operatoren (unär)"));
            tokenDescriptions.Add(new TokenDescription(@"[a-zA-Z]+([0-9]*[a-zA-Z]*_*])*", "Bezeichner"));
            tokenDescriptions.Add(new TokenDescription(@"[()]", "Klammern"));


		}
	}
}
