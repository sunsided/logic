using System;

namespace Logic.Nodes.Attributes
{
    /// <summary>
    /// Markierung für einen Operator
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class UnaryOperatorAttribute : NodeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryOperatorAttribute"/> class.
        /// </summary>
        /// <param name="nodeType">The type.</param>
        /// <remarks></remarks>
        public UnaryOperatorAttribute(Type nodeType)
            : base(nodeType)
        {
        }
    }
}
