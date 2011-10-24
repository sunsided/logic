using System;

namespace Logic.Nodes.Attributes
{
    /// <summary>
    /// Markierung für einen Operator
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class BinaryOperatorAttribute : NodeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorAttribute"/> class.
        /// </summary>
        /// <param name="nodeType">The type.</param>
        /// <remarks></remarks>
        public BinaryOperatorAttribute(Type nodeType)
            : base(nodeType)
        {
        }
    }
}
