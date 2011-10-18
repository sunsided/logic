namespace Logic.LanguageParser.Descriptions
{
    /// <summary>
    /// Token für einen unären Operator
    /// </summary>
    public abstract class UnaryOperatorToken : OperatorToken
    {
        /// <summary>
        /// Gibt an, ob es sich um eine Prefix-Negation handelt
        /// </summary>
        public bool IsPrefix { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryOperatorToken"/> class.
        /// </summary>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="description">The description.</param>
        /// <param name="isPrefix">if set to <c>true</c> [is prefix].</param>
        /// <remarks></remarks>
        protected UnaryOperatorToken(string regularExpression, string description, bool isPrefix) : base(regularExpression, description)
        {
            IsPrefix = isPrefix;
        }
    }
}
