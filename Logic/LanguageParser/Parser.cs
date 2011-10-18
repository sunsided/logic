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

            // Gleichung so lange durchlaufen, bis keine weiteren Gruppen/Token mehr
            // gefunden werden.
            string strippedEquation = equation;
            int marchingIndex = 0;
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
                tokenList.Add(new Token(match.Value, description, marchingIndex));

                // Gleichung vorbereiten
                PrepareEquationAndUpdateIndex(match, ref strippedEquation, ref marchingIndex);
            }

            return tokenList;
        }

        /// <summary>
        /// Bereitet die Gleichung für die nächste Iteration vor und aktualisiert den marching index
        /// </summary>
        /// <param name="match">Der zuletzt gefundene Treffer</param>
        /// <param name="strippedEquation">Die Eingabe, die für die Auswertung benutzt wird</param>
        /// <param name="marchingIndex">Der zu aktualisierende Index</param>
        private static void PrepareEquationAndUpdateIndex(Match match, ref string strippedEquation, ref int marchingIndex)
        {
            // Eben ermittelten Teil von der Eingabe abziehen
            strippedEquation = strippedEquation.Substring(match.Index + match.Length);

            // Ermitteln, wie viele Leerzeichen vom Anfang entfernt werden müssen
            // und diese zur Verrechnung mit dem marching index merken
            string spacesRemoved = strippedEquation.TrimStart();
            int numberOfSpacesRemoved = strippedEquation.Length - spacesRemoved.Length;
            strippedEquation = spacesRemoved;

            // Index hochzählen
            marchingIndex += match.Index + match.Length + numberOfSpacesRemoved;
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
