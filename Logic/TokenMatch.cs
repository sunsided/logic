using System;
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
        /// Der Typ des Tokens
        /// </summary>
        public TokenType Type { [Pure] get { return Description.Type; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenMatch"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="token">The token.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public TokenMatch(int index, string token, TokenDescription description)
        {
            Contract.Requires(index >= 0, "Index muss größer oder gleich 0 sein");
            Contract.Requires(!String.IsNullOrWhiteSpace(token), "Token darf nicht leer sein");
            Contract.Requires(description != null, "Description darf nicht null sein");

            Index = index;
            Token = token;
            Description = description;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return String.Format("{2} @ {0}:{1}, \"{3}\"", Index, Index + Length, Description.Description, Token);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks></remarks>
        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ Description.GetHashCode();
        }
    }
}
