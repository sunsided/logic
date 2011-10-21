using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Logic
{
    /// <summary>
    /// Der Parser
    /// </summary>
    public sealed class Parser
    {
        /// <summary>
        /// Die Beschreibungen
        /// </summary>
        private readonly HashSet<TokenDescription> _descriptions = new HashSet<TokenDescription>();

        /// <summary>
        /// Die Anzahl der registrierten Beschreibungen
        /// </summary>
        public int Count { [Pure] get { return _descriptions.Count; } }

        /// <summary>
        /// Fügt eine Beschreibung zum Satz hinzu
        /// </summary>
        /// <param name="description">Die hinzuzufügende Beschreibung</param>
        /// <returns>Die hinzuzufügende Beschreibung (method chaining)</returns>
        public TokenDescription AddDescription(TokenDescription description)
        {
            Contract.Requires(description != null, "Description darf nicht null sein");
            Contract.Ensures(Contract.Result<TokenDescription>() != null);

            _descriptions.Add(description);
            return description;
        }

        /// <summary>
        /// Fügt eine Beschreibung zum Satz hinzu
        /// </summary>
        /// <param name="descriptionText">Die hinzuzufügende Beschreibung</param>
        /// <returns>Die hinzuzufügende Beschreibung (method chaining)</returns>
        public TokenDescription AddDescription(string descriptionText, TokenType type)
        {
            Contract.Requires(descriptionText != null, "Beschreibungstext darf nicht null sein");
            Contract.Ensures(Contract.Result<TokenDescription>() != null);

            TokenDescription td = new TokenDescription(descriptionText, type);
            _descriptions.Add(td);
            return td;
        }

        /// <summary>
        /// Wertet eine Sequenz aus
        /// </summary>
        /// <param name="sequence">Die Sequenz</param>
        /// <returns>Die Liste der Token</returns>
        public IList<TokenMatch> Parse(string sequence)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(sequence), "Sequenz darf nicht leer sein");
            Contract.Ensures(Contract.Result<IList<TokenDescription>>() != null);
            List<TokenMatch> tokens = new List<TokenMatch>();

            // Vorbereitung
            IList<TokenMatch> localMatches = new List<TokenMatch>();

            // Sequenz durchlaufen, bis nichts mehr zu ermitteln ist
            int marchingIndex = 0;
            while (marchingIndex < sequence.Length)
            {
                // Alle Beschreibungen durchlaufen und Treffer ermitteln
                localMatches.Clear();
                foreach (var tokenDescription in _descriptions)
                {
                    TokenMatch localMatch;
                    if (!tokenDescription.IsMatch(sequence, marchingIndex, out localMatch)) continue;
                    localMatches.Add(localMatch);
                }

                // Sanity check
                Contract.Assume(!(localMatches.Count > 1), "Es wurde mehr als eine Tokenbeschreibung gefunden and Index " + marchingIndex);
                Contract.Assume(localMatches.Count == 1, "Es wurde keine Tokenbeschreibung gefunden an Index " + marchingIndex);

                // Token extrahieren
                TokenMatch match = localMatches[0];
                tokens.Add(match);

                // Hochzählen
                marchingIndex = match.Index + match.Length;
            }
            return tokens;
        }


        #region Code Contracts

        /// <summary>
        /// Vertragsinvariante
        /// </summary>
        [ContractInvariantMethod]
        // ReSharper disable UnusedMember.Local
        private void ContractInvariant()
        // ReSharper restore UnusedMember.Local
        {
            Contract.Invariant(_descriptions != null);
        }

        #endregion
    }
}
