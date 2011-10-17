namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Group Opening Token
    /// </summary>
    public sealed class GroupCloseToken : TokenDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCloseToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public GroupCloseToken(string regularExpression, string description)
            : base(regularExpression, description)
        {
        }
    }
}
