namespace Logic.Nodes.UnaryOperators
{
    /// <summary>
    /// XOR-Knoten
    /// </summary>
    public sealed class NotNode : UnaryOperatorNode
    {
        /// <summary>
        /// Wertet diesen Knoten aus
        /// </summary>
        /// <returns>Der Wahrheitswert dieses Knotens</returns>
        /// <remarks></remarks>
        public override bool Evaluate()
        {
            bool value = Node.Evaluate();
            return !value;
        }
    }
}
