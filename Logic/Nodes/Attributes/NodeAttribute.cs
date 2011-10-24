using System;
using System.Diagnostics.Contracts;

namespace Logic.Nodes.Attributes
{
    /// <summary>
    /// Markierung für einen Operator
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal abstract class NodeAttribute : Attribute
    {
        /// <summary>
        /// Der Typ
        /// </summary>
        public Type Type { [Pure] get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeAttribute"/> class.
        /// </summary>
        /// <param name="nodeType">Type of the node.</param>
        /// <remarks></remarks>
        protected NodeAttribute(Type nodeType)
        {
            Type = nodeType;
        }
    }
}
