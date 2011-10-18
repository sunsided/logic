namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Token für einen binären Operator
    /// </summary>
    public sealed class BinaryOperatorToken : OperatorToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public BinaryOperatorToken(string regularExpression, string description) : base(regularExpression, description)
        {
        }
    }
}
