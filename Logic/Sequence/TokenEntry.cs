using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Logic.Sequence
{
    /// <summary>
    /// Ein Token-Eintrag
    /// </summary>
    [DebuggerDisplay("token: {Match}")]
    public sealed class TokenEntry : TokenSequenceEntry
    {
        /// <summary>
        /// Die zugehörige TokenMatch-Instanz
        /// </summary>
        public TokenMatch Match { [Pure] get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenEntry"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <remarks></remarks>
        public TokenEntry(TokenMatch match)
        {
            Contract.Requires(match != null, "Match darf nicht null sein");
            Match = match;
        }

        /// <summary>
        /// Ermittelt, ob es sich um eine NOT-Operation handelt
        /// </summary>
        /// <returns></returns>
        public bool IsNotOperation()
        {
            TokenType type = Match.Type;
            return type == TokenType.Not || type == TokenType.NotReverse;
        }

        /// <summary>
        /// Ermittelt, ob es sich um eine ODER-Operation handelt
        /// </summary>
        /// <returns></returns>
        public bool IsBinaryOperation()
        {
            return IsAndOperation() || IsOrOperation();
        }

        /// <summary>
        /// Ermittelt, ob es sich um eine ODER-Operation handelt
        /// </summary>
        /// <returns></returns>
        public bool IsAndOperation()
        {
            TokenType type = Match.Type;
            return type == TokenType.And || type == TokenType.Nand;
        }

        /// <summary>
        /// Ermittelt, ob es sich um eine ODER-Operation handelt
        /// </summary>
        /// <returns></returns>
        public bool IsOrOperation()
        {
            TokenType type = Match.Type;
            return type == TokenType.Or || type == TokenType.Xnor || type == TokenType.Xor || type == TokenType.Nor;
        }
    }
}
