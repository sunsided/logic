using System.Diagnostics.Contracts;

namespace Logic.Nodes
{
    /// <summary>
    /// Ein Term
    /// </summary>
    public sealed class TermNode : TokenNode
    {
        /// <summary>
        /// Der Match
        /// </summary>
        public TokenMatch Match { [Pure] get; set; }
    }
}
