using System.Diagnostics.Contracts;

namespace Logic.Nodes
{
    /// <summary>
    /// Negation
    /// </summary>
    public abstract class UnaryOperatorNode : TokenNode
    {
        /// <summary>
        /// Der Match
        /// </summary>
        public TokenMatch Match { [Pure] get; set; }

        /// <summary>
        /// Der Node
        /// </summary>
        public TokenNode Node { get; set; }
    }
}
