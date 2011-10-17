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
        /// Initializes a new instance of the <see cref="TokenDescription"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <remarks></remarks>
        public TokenDescription(string regularExpression)
        {
            Contract.Requires(!String.IsNullOrEmpty(regularExpression), "The regular expression must not be empty");

            _expression = new Regex(regularExpression, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
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
