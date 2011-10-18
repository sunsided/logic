namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Negation
    /// </summary>
    public sealed class PostfixNegationToken : UnaryOperatorToken, INegationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixNegationToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public PostfixNegationToken(string regularExpression, string description)
            : base(regularExpression, description, false)
        {
        }
    }
}
