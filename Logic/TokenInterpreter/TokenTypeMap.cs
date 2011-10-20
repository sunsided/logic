using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Logic.TokenInterpreter
{
    /// <summary>
    /// Die Map der Token-Typen
    /// </summary>
    internal sealed class TokenTypeMap
    {
        /// <summary>
        /// Liste der Token-Einträge
        /// </summary>
        private readonly List<TokenTypeMapEntry> entries = new List<TokenTypeMapEntry>();

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void Add(TokenTypeMapEntry entry)
        {
            Contract.Requires(entry != null, "Entry darf nicht null sein");
            entries.Add(entry);
        }

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Type GetTokenType(string token)
        {
            Contract.Requires(token != null);
            return entries.Where(t => t.IsMatch(token)).Select(t => t.TokenType).FirstOrDefault();
        }
    }
}
