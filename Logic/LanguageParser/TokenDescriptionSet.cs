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
        /// Entfernt whitespace aus der Eingabe
        /// </summary>
        /// <param name="value">Die Eingabe</param>
        /// <returns></returns>
        private string RemoveWhiteSpace(string value)
        {
            Contract.Requires(value != null, "Value must not be null");
            Contract.Ensures(Contract.Result<string>() != null);
            return value.Replace(" ", "").Replace("\t", ""); // TODO: Naiven Ansatz durch Regex o.ä. ersetzen
        }

        /// <summary>
        /// Parses the specified equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        /// <returns></returns>
        public IList<Token> Parse(string equation)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(equation), "Equation must not be null");
            equation = RemoveWhiteSpace(equation);
            List<Token> tokenList = new List<Token>();

            // Durchlaufen, bis keine weiteren Token mehr gefunden werden können
            KeyValuePair<TokenDescription, int>? lastTokenDescription = null; // TODO: Umwandeln in eigene Struktur/Klasse
            for (int currentCharIndex = 0; currentCharIndex <= equation.Length; ++currentCharIndex)
            {
                string substring = equation[currentCharIndex].ToString();
                List<TokenDescription> possibleToken = _tokenDescriptions.Where(desc => desc.Expression.IsMatch(substring)).ToList();
                
                // Sicherstellen, dass nur eine Tokenbeschreibung zutrifft
                if (possibleToken.Count > 1) throw new ParserException("Could not evaluate equation. Multiple Token matched at index " + currentCharIndex + " for substring '" + substring + "'.", currentCharIndex, substring);
                if (possibleToken.Count == 0) throw new ParserException("Could not evaluate equation. Token mismatch at index " + currentCharIndex + " (" + substring + ")", currentCharIndex, substring);

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
                        // Bezeichner auswerten
                        var tokenString = ExtractTokenFromEquation(equation, currentCharIndex, lastTokenDescription.Value);
                        tokenList.Add(new Token(tokenString, lastTokenDescription.Value.Key));

                        // Eben gefundenes Token merken
                        lastTokenDescription = new KeyValuePair<TokenDescription, int>(currentTokenDescription, currentCharIndex);
                    }
                }
                else // Initialfall
                {
                    // Initiales Token schreiben
                    lastTokenDescription = new KeyValuePair<TokenDescription, int>(currentTokenDescription, currentCharIndex);
                }
            }

            return tokenList;
        }

        /// <summary>
        /// Extrahiert ein Token aus der Eingabe
        /// </summary>
        /// <param name="equation">Die Eingabe</param>
        /// <param name="currentCharIndex">Der aktuelle Zeichenindex</param>
        /// <param name="tokenDescription">Die Beschreibung des Tokens</param>
        /// <returns>Das Token</returns>
        private static string ExtractTokenFromEquation(string equation, int currentCharIndex, KeyValuePair<TokenDescription, int> tokenDescription)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(equation), "Equation must not be null");
            Contract.Requires(currentCharIndex >= 1, "Aktueller Zeichenindex muss größer als 0 sein");
            Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()), "Das Token darf nicht null sein");

            // Bereich ermitteln
            int startIndex = tokenDescription.Value;
            int endIndex = currentCharIndex - 1; // (ist im worst case identisch mit startIndex)
            int length = endIndex - startIndex + 1;
            Contract.Assume(length > 0, "Token length must be greater than zero");

            // Token abziehen
            string token = equation.Substring(startIndex, length);
            Contract.Assume(!String.IsNullOrWhiteSpace(token), "Token must not be empty");
            return token;
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
