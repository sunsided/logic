using System.Diagnostics.Contracts;

namespace Logic
{
    /// <summary>
    /// Ein ermitteltes Token
    /// </summary>
    public sealed class TokenMatch
    {
        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <remarks></remarks>
        public int Index { [Pure] get; private set; }

        /// <summary>
        /// Die Länge des Tokens
        /// </summary>
        public int Length { [Pure] get { return (Token ?? "").Length; } }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <remarks></remarks>
        public string Token { [Pure] get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <remarks></remarks>
        public TokenDescription Description { [Pure]  get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenMatch"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="token">The token.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public TokenMatch(int index, string token, TokenDescription description)
        {
            Index = index;
            Token = token;
            Description = description;
        }
    }
}
