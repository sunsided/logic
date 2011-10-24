using System.Collections.Generic;
using Logic.Nodes;
using Logic.Sequence;

namespace Logic
{
    /// <summary>
    /// Rundum-Glücklich-Logikparser
    /// </summary>
    public static class LogicParser
    {
        /// <summary>
        /// Erzeugt den Ausdrucksbaum
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        public static TokenTree Parse(string equation)
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
            
            // Parsen
            IList<TokenSequenceEntry> result = parser.Parse(equation);

            // Hierarchiebaum erzeugen
            TreeGenerator generator = new TreeGenerator();
            return generator.GenerateHierarchy(result);
        }
    }
}
