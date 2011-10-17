using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Logic.LanguageParser
{
    /// <summary>
    /// Eine Token-Beschreibung
    /// </summary>
    [DebuggerDisplay("{_expression}")]
    public sealed class TokenDescription
    {
        /// <summary>
        /// Die Regular Expression, die das Token beschreibt
        /// </summary>
        private readonly Regex _expression;

        /// <summary>
        /// Die Regex
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// Die Bezeichnung des Tokens
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenDescription"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public TokenDescription(string regularExpression, string description)
        {
            Contract.Requires(!String.IsNullOrEmpty(regularExpression), "The regular expression must not be empty");
            Contract.Requires(!String.IsNullOrEmpty(description), "The description must not be empty");

            Expression = regularExpression;
            _expression = new Regex(regularExpression, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            Description = description;
        }
        
        #region Contracts

        /// <summary>
        /// Vertragsinvariante
        /// </summary>
        [ContractInvariantMethod]
        private void ContractInvariant()
        {
            Contract.Invariant(_expression != null, "Expression must not be null");
        }

        #endregion
    }
}
