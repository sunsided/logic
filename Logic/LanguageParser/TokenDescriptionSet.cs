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
            Contract.Requires(equation.Length >= 1, "Equation must contain at least one element");
            equation = RemoveWhiteSpace(equation);
            List<Token> tokenList = new List<Token>();

            // TODO: Matchen mittels Regex-Gruppen!
            // TODO: Wenn Regex-Gruppe gefunden, Substring von equation abschneiden und repeat, bis Eingang leer

            // Durchlaufen, bis keine weiteren Token mehr gefunden werden können
            KeyValuePair<TokenDescription, int>? lastTokenDescription = null; // TODO: Umwandeln in eigene Struktur/Klasse

            for (int currentCharIndex = 0; currentCharIndex <= equation.Length; ++currentCharIndex)
            {
                string substring = null;
                TokenDescription currentTokenDescription = null;

                // Substring so lange aufziehen, bis keine Tokenbeschreibung mehr passt
                if (currentCharIndex < equation.Length)
                {
                    substring = ExtractProtoToken(equation, currentCharIndex, lastTokenDescription);
                    currentTokenDescription = GetMatchingTokenDescription(currentCharIndex, substring);
                }

                // Wenn die Verarbeitung fehlschlägt, ist das gültige Token beendet und ein neues beginnt
                if (currentTokenDescription == null)
                {
                    // Bezeichner auswerten
                    Contract.Assume(lastTokenDescription != null);
                    string tokenString = ExtractTokenFromEquation(equation, currentCharIndex, lastTokenDescription.Value);
                    tokenList.Add(new Token(tokenString, lastTokenDescription.Value.Key));

                    // Abbruchkriterium
                    if (currentCharIndex == equation.Length) break;

                    // Neues Token auswerten
                    string newSubstring = equation.Substring(currentCharIndex, 1);
                    currentTokenDescription = GetMatchingTokenDescription(currentCharIndex, newSubstring);
                    if (currentTokenDescription == null) throw new ParserException("Could not evaluate equation. Token mismatch at index " + currentCharIndex + " (" + substring + ")", currentCharIndex, substring);
                    substring = newSubstring;
                }

                // Token beziehen
                Contract.Assume(currentTokenDescription != null, "The selected token description was null.");

                // Wenn ein Token gefunden wurde - Regelfall
                if (lastTokenDescription == null)
                {
                    lastTokenDescription = new KeyValuePair<TokenDescription, int>(currentTokenDescription, 0);
                }
                else if (currentTokenDescription != lastTokenDescription.Value.Key)
                {
                    lastTokenDescription = new KeyValuePair<TokenDescription, int>(currentTokenDescription, currentCharIndex);
                }
            }

            return tokenList;
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
        /// Extracts the proto token.
        /// </summary>
        /// <param name="equation">The equation.</param>
        /// <param name="currentCharIndex">Index of the current char.</param>
        /// <param name="tokenDescription">The token description.</param>
        /// <returns></returns>
        private string ExtractProtoToken(string equation, int currentCharIndex, KeyValuePair<TokenDescription, int>? tokenDescription)
        {
            int startIndex = tokenDescription != null ? tokenDescription.Value.Value : 0;
            int length = tokenDescription != null ? (currentCharIndex - tokenDescription.Value.Value + 1) : 1;
            return equation.Substring(startIndex, length);
        }

        /// <summary>
        /// Gets the matching token description.
        /// </summary>
        /// <param name="currentCharIndex">Index of the current char.</param>
        /// <param name="substring">The substring.</param>
        /// <returns>Das Token oder <c>null</c>, falls kein passendes Token gefunden wurde</returns>
        /// <exception cref="ParserException">Es trafen mehrere Tokenbeschreibungen auf das Token zu</exception>
        private TokenDescription GetMatchingTokenDescription(int currentCharIndex, string substring)
        {
            // Gültige Tokenbeschreibung ermitteln
            List<TokenDescription> possibleToken = _tokenDescriptions.Where(desc => desc.Expression.IsMatch(substring)).ToList();

            // Sicherstellen, dass nur eine Tokenbeschreibung zutrifft
            if (possibleToken.Count > 1) throw new ParserException("Could not evaluate equation. Multiple Token matched at index " + currentCharIndex + " for substring '" + substring + "'.", currentCharIndex, substring);
            
            // Token oder null zurückgeben
            return possibleToken.Count > 0 ? possibleToken[0] : null;
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
