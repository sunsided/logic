using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Logic.LanguageParser
{
    /// <summary>
    /// Ein Token
    /// </summary>
    [DebuggerDisplay("{SubExpression} {OriginDescription.Description}")]
    public sealed class Token
    {
        /// <summary>
        /// Der Ausdruck
        /// </summary>
        public string SubExpression { get; private set; }

        /// <summary>
        /// Das zugehörige Beschreibung
        /// </summary>
        public TokenDescription OriginDescription { get; private set; }

        /// <summary>
        /// Gibt an, ob das Token identifiziert wurde
        /// </summary>
        public bool IsIdentified { [Pure] get { return OriginDescription != null; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="subExpression">The sub expression.</param>
        /// <param name="originDescription">The origin description.</param>
        public Token(string subExpression, TokenDescription originDescription)
            : this(subExpression)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(subExpression), "Unterausdruck darf nicht null sein");
            Contract.Requires(originDescription != null, "Stammausdruck darf nicht null sein");

            OriginDescription = originDescription;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="subExpression">The sub expression.</param>
        public Token(string subExpression)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(subExpression), "Unterausdruck darf nicht null sein");

            SubExpression = subExpression;
        }
    }
}
