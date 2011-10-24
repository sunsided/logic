using System.Diagnostics.Contracts;

namespace Logic.Nodes
{
    /// <summary>
    /// Ein Term
    /// </summary>
    public abstract class BinaryOperatorNode : TokenNode
    {
        /// <summary>
        /// Der Match
        /// </summary>
        public TokenMatch Match { [Pure] get; set; }

        /// <summary>
        /// Der Node
        /// </summary>
        public TokenNode LeftNode { get; set; }

        /// <summary>
        /// Der Node
        /// </summary>
        public TokenNode RightNode { get; set; }
    }
}
