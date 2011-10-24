namespace Logic.Nodes.BinaryOperators
{
    /// <summary>
    /// XNOR-Knoten
    /// </summary>
    public sealed class XnorNode : BinaryOperatorNode
    {
        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>Der Wahrheitswert dieses Knotens</returns>
        /// <remarks></remarks>
        public override bool Evaluate()
        {
            bool left = LeftNode.Evaluate();
            bool right = RightNode.Evaluate();
            return !(left ^ right);
        }
    }
}
