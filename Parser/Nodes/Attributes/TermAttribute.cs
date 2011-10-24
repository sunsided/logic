using System;

namespace Logic.Nodes.Attributes
{
    /// <summary>
    /// Markierung für einen Operator
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class TermAttribute : NodeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermAttribute"/> class.
        /// </summary>
        /// <param name="nodeType">The type.</param>
        /// <remarks></remarks>
        public TermAttribute(Type nodeType)
            : base(nodeType)
        {
        }
    }
}
