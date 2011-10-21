namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Token für einen binären Operator
    /// </summary>
    public sealed class NorOperatorToken : BinaryOperatorToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NorOperatorToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public NorOperatorToken(string regularExpression, string description)
            : base(regularExpression, description)
        {
        }
    }
}
