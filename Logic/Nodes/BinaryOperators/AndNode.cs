namespace Logic.Nodes.BinaryOperators
{
    /// <summary>
    /// AND-Knoten
    /// </summary>
    public sealed class AndNode : BinaryOperatorNode
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
            return left && right;
        }
    }
}
