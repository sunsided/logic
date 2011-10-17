namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Operator
    /// </summary>
    public sealed class OperatorToken : TokenDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public OperatorToken(string regularExpression, string description)
            : base(regularExpression, description)
        {
        }
    }
}
