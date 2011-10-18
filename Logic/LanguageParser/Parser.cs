using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logic.LanguageParser
{
    /// <summary>
    /// Satz an Tokenbeschreibungen
    /// </summary>
    public sealed class Parser
    {
        /// <summary>
        /// Die Beschreibungen
        /// </summary>
        private readonly IList<TokenDescription> _tokenDescriptions = new List<TokenDescription>();

        /// <summary>
        /// Liefert die Anzahl an registrierten Beschreibungen
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return _tokenDescriptions.Count; }}

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _tokenDescriptions.Clear();
        }

        /// <summary>
        /// Adds the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        public void Add(TokenDescription description)
        {
            Contract.Requires(description != null, "Description must not be null");
            _tokenDescriptions.Add(description);
        }

        /// <summary>
        /// Parses the specified equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        /// <returns></returns>
        public IList<Token> Parse(string equation)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(equation), "Equation must not be null");
            Contract.Requires(equation.Length >= 1, "Equation must contain at least one element");
            List<Token> tokenList = new List<Token>();

            // TODO: Matchen mittels Regex-Gruppen!
            // TODO: Wenn Regex-Gruppe gefunden, Substring von equation abschneiden und repeat, bis Eingang leer

            // Durchlaufen, bis keine weiteren Token mehr gefunden werden können
            KeyValuePair<TokenDescription, int>? lastTokenDescription = null; // TODO: Umwandeln in eigene Struktur/Klasse

            // Gleichung so lange durchlaufen, bis keine weiteren Gruppen/Token mehr
            // gefunden werden.
            string strippedEquation = equation;
            while (strippedEquation.Length > 0)
            {
                // Aus allen Beschreibungen eine finden, die passt
                List<TokenDescription> matches = _tokenDescriptions
                    .Where(desc => desc.Expression.IsMatch(strippedEquation))
                    .Select(desc => desc)
                    .ToList();
                if (matches.Count > 1) Trace.TraceWarning("Multiple token descriptions found for substring \"" + matches[0].Expression.Match(strippedEquation).Value + "\"");
                Contract.Assume(matches.Count == 1);               

                // Die gefundene Beschreibung auswerten
                TokenDescription description = matches[0];
                Match match = description.Expression.Match(strippedEquation);
                Contract.Assume(match.Success);
                
                // Token erzeugen
                tokenList.Add(new Token(match.Value, description));

                // Gleichung vorbereiten
                strippedEquation = strippedEquation.Substring(match.Index + match.Length).TrimStart();
            }

            return tokenList;
        }


        #region Contracts

        /// <summary>
        /// Vertragsinvariante
        /// </summary>
        [ContractInvariantMethod]
        private void ContractInvariant()
        {
            Contract.Invariant(_tokenDescriptions != null);
        }

        #endregion Contracts
    }
}
