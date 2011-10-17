namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Group Opening Token
    /// </summary>
    public sealed class GroupOpenToken : TokenDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupOpenToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public GroupOpenToken(string regularExpression, string description)
            : base(regularExpression, description)
        {
        }
    }
}
