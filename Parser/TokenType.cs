using Logic.Nodes;
using Logic.Nodes.Attributes;
using Logic.Nodes.BinaryOperators;
using Logic.Nodes.UnaryOperators;

namespace Logic
{
    /// <summary>
    /// Token-Typ
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Ein Term
        /// </summary>
        [Term(typeof(TermNode))]
        Term,

        /// <summary>
        /// AND
        /// </summary>
        [BinaryOperator(typeof(AndNode))]
        And,

        /// <summary>
        /// NAND
        /// </summary>
        [BinaryOperator(typeof(NandNode))]
        Nand,

        /// <summary>
        /// OR
        /// </summary>
        [BinaryOperator(typeof(OrNode))]
        Or,

        /// <summary>
        /// NOR
        /// </summary>
        [BinaryOperator(typeof(NorNode))]
        Nor,

        /// <summary>
        /// XOR
        /// </summary>
        [BinaryOperator(typeof(XorNode))]
        Xor,

        /// <summary>
        /// XNOR
        /// </summary>
        [BinaryOperator(typeof(XnorNode))]
        Xnor,

        /// <summary>
        /// NOT (vorwärts wirkend)
        /// </summary>
        [UnaryOperator(typeof(NotNode))]
        Not,

        /// <summary>
        /// NOT (rückwärts wirkend)
        /// </summary>
        NotReverse,

        /// <summary>
        /// Start einer Gruppe
        /// </summary>
        GroupStart,

        /// <summary>
        /// Ende einer Gruppe
        /// </summary>
        GroupEnd,
    }
}
