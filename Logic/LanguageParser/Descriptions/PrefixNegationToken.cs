namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Negation
    /// </summary>
    public sealed class PrefixNegationToken : UnaryOperatorToken, INegationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixNegationToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public PrefixNegationToken(string regularExpression, string description) 
            : base(regularExpression, description, true)
        {
        }
    }
}
