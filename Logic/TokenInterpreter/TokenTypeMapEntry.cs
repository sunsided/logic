using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Logic.TokenInterpreter.TokenTypes;

namespace Logic.TokenInterpreter
{
    /// <summary>
    /// Eintrag in der Token-Map
    /// </summary>
    internal sealed class TokenTypeMapEntry
    {
        /// <summary>
        /// Gleichwertige Begriffe für dieses Token
        /// </summary>
        private readonly HashSet<string> Token = new HashSet<string>();

        /// <summary>
        /// Der Token-Typ
        /// </summary>
        public Type TokenType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenTypeMapEntry"/> class.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="token">The token.</param>
        /// <param name="additionalTokens">The additional tokens.</param>
        public TokenTypeMapEntry(Type tokenType, string token, params string[] additionalTokens)
        {
            Contract.Requires(typeof (TokenNode).IsAssignableFrom(tokenType), "TokenType muss vom Typ TokenNode sein");
            Contract.Requires(!String.IsNullOrWhiteSpace(token));
            Contract.Requires(additionalTokens != null);
            Contract.Requires(Contract.ForAll(additionalTokens, t => !String.IsNullOrWhiteSpace(t)));

            TokenType = tokenType;

            // Token hinzufügen
            Token.Add(token);
            for(int i=0; i<additionalTokens.Length; ++i)
            {
                Token.Add(additionalTokens[i]);
            }
        }

        /// <summary>
        /// Ermittelt, ob das angegebene Token zu diesem Eintrag gehört
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// 	<c>true</c> if the specified token is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(string token)
        {
            return Token.Contains(token, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
