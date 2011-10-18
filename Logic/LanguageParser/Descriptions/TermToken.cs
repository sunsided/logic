namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Term
    /// </summary>
    public sealed class TermToken : TokenDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public TermToken(string regularExpression, string description)
            : base(regularExpression, description)
        {
        }
    }
}
