using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logic
{
    /// <summary>
    /// Helferklasse für das Erstellen von Regular Expressions für den Parser
    /// </summary>
    public static class ParserRegexBuilder
    {
        /// <summary>
        /// Die Default-Optionen für die Regex
        /// </summary>
        private const RegexOptions DefaultOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

        /// <summary>
        /// Erzeugt die Regex.
        /// </summary>
        /// <param name="token">Das Token</param>
        /// <param name="additionalTokens">Die zusätzlichen Token</param>
        /// <returns>Die Regex, die auf alle genannten Token zutrifft</returns>
        public static Regex Build(string token, params string[] additionalTokens)
        {
            Contract.Requires(token != null, "Token darf nicht null sein");
            Contract.Requires(additionalTokens != null, "Zusätzliche Token dürfen nicht null sein");
            Contract.Requires(Contract.ForAll(additionalTokens, t => t != null), "Zusätzliche Token dürfen nicht null sein");
            Contract.Ensures(Contract.Result<Regex>() != null);

            // Wordtliste erzeugen
            IEnumerable<string> words = CreateRegexSafeWordList(token, additionalTokens);

            // Regex erzeugen
            string regularExpression = "^(?<token>" + String.Join("|", words) + ")";
            return new Regex(regularExpression, DefaultOptions);
        }

        /// <summary>
        /// Erzeugt die Regex.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns>Die Regex, die auf alle genannten Token zutrifft</returns>
        /// <remarks></remarks>
        public static Regex Build(IList<string> tokens)
        {
            Contract.Requires(tokens != null, "Token darf nicht null sein");
            Contract.Requires(Contract.ForAll(tokens, t => t != null), "Token dürfen nicht null sein");
            Contract.Ensures(Contract.Result<Regex>() != null);

            // Wordtliste erzeugen
            IList<string> words = CreateRegexSafeWordList(tokens);

            // Regex erzeugen
            string regularExpression = "^(?<token>" + String.Join("|", words) + ")";
            return new Regex(regularExpression, DefaultOptions);
        }

        /// <summary>
        /// Erzeugt die Regex.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="reservedWords"></param>
        /// <returns>Die Regex, die auf alle genannten Token zutrifft</returns>
        /// <remarks></remarks>
        public static Regex Build(IList<string> tokens, IList<string> reservedWords)
        {
            Contract.Requires(tokens != null, "Token darf nicht null sein");
            Contract.Requires(Contract.ForAll(tokens, t => t != null), "Token dürfen nicht null sein");
            Contract.Requires(reservedWords != null, "reservedWords darf nicht null sein");
            Contract.Requires(Contract.ForAll(reservedWords, t => t != null), "Reservierte Wörter dürfen nicht null sein");
            Contract.Ensures(Contract.Result<Regex>() != null);

            // Wordtliste erzeugen
            IList<string> words = CreateRegexSafeWordList(tokens);
            reservedWords = CreateRegexSafeWordList(reservedWords);

            // Regex erzeugen
            string regularExpression = "^(!?(" + String.Join("|", reservedWords) + "))(?<token>" +
                                       String.Join("|", words) + ")";
            return new Regex(regularExpression, DefaultOptions);
        }

        /// <summary>
        /// Erzeugt eine Liste von escapten Begriffen für die Auswertung mittels Regex
        /// </summary>
        /// <param name="token">Das Token</param>
        /// <param name="additionalTokens">Die zusätzlichen Token</param>
        /// <returns></returns>
        private static IEnumerable<string> CreateRegexSafeWordList(string token, params string[] additionalTokens)
        {
            Contract.Requires(token != null, "Token darf nicht null sein");
            Contract.Requires(additionalTokens != null, "Zusätzliche Token dürfen nicht null sein");
            Contract.Requires(Contract.ForAll(additionalTokens, t => t != null), "Zusätzliche Token dürfen nicht null sein");
            Contract.Ensures(Contract.Result<IList<string>>() != null);

            List<string> words = new List<string>(additionalTokens.Length + 1);
            words.Add(Regex.Escape(token));
            words.AddRange(additionalTokens.Select(Regex.Escape));

            return words;
        }

        /// <summary>
        /// Erzeugt eine Liste von escapten Begriffen für die Auswertung mittels Regex
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IList<string> CreateRegexSafeWordList(IList<string> tokens)
        {
            Contract.Requires(tokens != null, "Token darf nicht null sein");
            Contract.Requires(Contract.ForAll(tokens, t => t != null), "Zusätzliche Token dürfen nicht null sein");
            Contract.Ensures(Contract.Result<IList<string>>() != null);

            List<string> words = new List<string>(tokens.Count);
            words.AddRange(tokens.Select(Regex.Escape));

            return words;
        }
    }
}
