using System.Diagnostics.Contracts;

namespace Logic.TokenInterpreter.TokenTypes
{
    /// <summary>
    /// Binärer Operator
    /// </summary>
    public abstract class UnaryOperator : TokenNode
    {
        /// <summary>
        /// Der Kindknoten
        /// </summary>
        public TokenNode ChildNode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryOperator"/> class.
        /// </summary>
        /// <param name="childNode">The child node.</param>
        protected UnaryOperator(TokenNode childNode)
        {
            Contract.Requires(childNode != null, "Knoten darf nicht null sein");

            ChildNode = childNode;
        }
    }
}
