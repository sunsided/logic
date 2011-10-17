using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
