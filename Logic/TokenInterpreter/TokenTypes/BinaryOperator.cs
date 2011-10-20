using System.Diagnostics.Contracts;

namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// Binärer Operator
    /// </summary>
    public abstract class BinaryOperator : TokenNode
    {
        /// <summary>
        /// Die Ordnung der Operation über anderen binäre Operatoren
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Der linke Knoten
        /// </summary>
        public TokenNode LeftNode { get; private set; }

        /// <summary>
        /// Der rechte Knoten
        /// </summary>
        public TokenNode RightNode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperator"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="leftNode">The left node.</param>
        /// <param name="rightNode">The right node.</param>
        protected BinaryOperator(int order, TokenNode leftNode, TokenNode rightNode)
        {
            Contract.Requires(leftNode != null, "Knoten darf nicht null sein");
            Contract.Requires(rightNode != null, "Knoten darf nicht null sein");

            Order = order;
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
}
