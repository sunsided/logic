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
    }
}
