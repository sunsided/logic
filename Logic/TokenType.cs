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
        Term,

        /// <summary>
        /// AND
        /// </summary>
        And,

        /// <summary>
        /// NAND
        /// </summary>
        Nand,

        /// <summary>
        /// OR
        /// </summary>
        Or,

        /// <summary>
        /// NOR
        /// </summary>
        Nor,

        /// <summary>
        /// XOR
        /// </summary>
        Xor,

        /// <summary>
        /// XNOR
        /// </summary>
        Xnor,

        /// <summary>
        /// NOT (vorwärts wirkend)
        /// </summary>
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
