using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

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
            _descriptions.Add(description);
            return description;
        }

        /// <summary>
        /// Wertet eine Sequenz aus
        /// </summary>
        /// <param name="sequence">Die Sequenz</param>
        /// <returns>Die Liste der Token</returns>
        public IList<TokenDescription> Parse(string sequence)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(sequence), "Sequenz darf nicht leer sein");
            List<TokenDescription> tokenDescriptions = new List<TokenDescription>();
            
            /*
            sequence = sequence.Trim();
            while (sequence.Length > 0)
            {
                List<TokenDescription> foundDescriptions = _descriptions.Where(description => description.IsMatch(sequence)).ToList();
                Contract.Assume(!(foundDescriptions.Count > 1), "Es wurde mehr als eine Tokenbeschreibung gefunden.");
                Contract.Assume(foundDescriptions.Count == 1, "Es wurde keine Tokenbeschreibung gefunden.");

                // Beschreibung extrahieren
                TokenDescription desciption = foundDescriptions[0];
                tokenDescriptions.Add(desciption);
            }
            */
            return null;
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
