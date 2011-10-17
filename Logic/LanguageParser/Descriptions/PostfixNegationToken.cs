namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Negation
    /// </summary>
    public sealed class PostfixNegationToken : TokenDescription, INegationToken
    {
        /// <summary>
        /// Gibt an, ob es sich um eine Prefix-Negation handelt
        /// </summary>
        public bool IsPrefix { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixNegationToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public PostfixNegationToken(string regularExpression, string description)
            : base(regularExpression, description)
        {
        }
    }
}
