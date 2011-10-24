using System.Diagnostics.Contracts;

namespace Logic.Nodes
{
    /// <summary>
    /// Negation
    /// </summary>
    public abstract class UnaryOperatorNode : TokenNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        internal UnaryOperatorNode() {}

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
