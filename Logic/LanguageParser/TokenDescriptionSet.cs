using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Logic.LanguageParser
{
    /// <summary>
    /// Satz an Tokenbeschreibungen
    /// </summary>
    public sealed class TokenDescriptionSet
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
        /// Adds the specified description.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        public void Add(string regularExpression, string description)
        {
            Contract.Requires(!String.IsNullOrEmpty(regularExpression), "The regular expression must not be empty");
            Contract.Requires(!String.IsNullOrEmpty(description), "The description must not be empty");
            Add(new TokenDescription(regularExpression, description));
        }

        /// <summary>
        /// Parses the specified equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        /// <returns></returns>
        public IList<Token> Parse(string equation)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(equation), "Equation must not be null");
            List<Token> tokenList = new List<Token>();

            // Initiales Token hinzufügen
            tokenList.Add(new Token(equation));

            // Durchlaufen, bis keine weiteren Token mehr gefunden werden können
            KeyValuePair<TokenDescription, int>? lastTokenDescription = null; // TODO: Umwandeln in eigene Struktur/Klasse
            for (int ci = 0; ci <= equation.Length; ++ci)
            {
                string substring = equation[ci].ToString();
                List<TokenDescription> possibleToken = _tokenDescriptions.Where(desc => desc.Expression.IsMatch(c)).ToList();
                
                // Sicherstellen, dass nur eine Tokenbeschreibung zutrifft
                if (possibleToken.Count > 1) throw new ParserException("Could not evaluate equation. Multiple Token matched for substring '" + substring + "'.");

                // Token beziehen
                TokenDescription currentTokenDescription = possibleToken[0];
                Contract.Assume(currentTokenDescription != null, "The selected token description was null.");

                // Wenn ein Token gefunden wurde - Regelfall
                // (es wird nur der Initialfall ausgeschlossen)
                if (lastTokenDescription != null)
                {
                    // Prüfen, ob eine neue Tokenbeschreibung gefunden wurde,
                    // d.h. das vorherige Token beendet wurde, falls vorhanden
                    if (currentTokenDescription != lastTokenDescription.Value.Key)
                    {

                        // Eben gefundenes Token merken
                        lastTokenDescription = new KeyValuePair<TokenDescription, int>(currentTokenDescription, ci);
                    }
                }
                else
                {
                    // Initiales Token schreiben
                    lastTokenDescription = new KeyValuePair<TokenDescription, int>(currentTokenDescription, ci);
                }
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
