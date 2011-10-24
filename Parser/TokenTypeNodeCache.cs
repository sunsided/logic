using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Logic.Nodes;
using Logic.Nodes.Attributes;

namespace Logic
{
    /// <summary>
    /// Der Token-Node-Cache
    /// </summary>
    internal static class TokenTypeNodeCache
    {
        /// <summary>
        /// Der Lookup der Typen
        /// </summary>
        private static readonly Dictionary<TokenType, Type> Map = new Dictionary<TokenType, Type>();

        /// <summary>
        /// Liste generieren
        /// </summary>
        static TokenTypeNodeCache()
        {
            // Alle Felder durchlaufen und nach Attributen durchforsten
            Type t = typeof (TokenType);
            FieldInfo[] fields = t.GetFields();

            for (int f=0; f<fields.Length; ++f)
            {
                FieldInfo field = fields[f];
                NodeAttribute[] attributes = (NodeAttribute[])field.GetCustomAttributes(typeof (NodeAttribute), true);
                if (attributes.Length == 0) continue;
                Contract.Assume(attributes.Length == 1);

                // Eintüten!
                TokenType type = (TokenType)Enum.Parse(t, field.Name);
                Map.Add(type, attributes[0].Type);
            }
        }

        /// <summary>
        /// Ermittelt den Node-Typen aus dem Token-Typen
        /// </summary>
        /// <param name="tokenType">Der Token-Typ</param>
        /// <returns>Der Node-Typ oder <c>null</c> im Fehlerfall</returns>
        public static Type GetNodeType(TokenType tokenType)
        {
            if (Map.ContainsKey(tokenType)) return Map[tokenType];
            return null;
        }

        /// <summary>
        /// Ermittelt den Node-Typen aus dem Token-Typen
        /// </summary>
        /// <param name="tokenType">Der Token-Typ</param>
        /// <returns>Der Node-Typ oder <c>null</c> im Fehlerfall</returns>
        public static TokenNode CreateNode(TokenType tokenType)
        {
            Type type = GetNodeType(tokenType);
            if (type == null) return null;
            return (TokenNode)Activator.CreateInstance(type);
        }
    }
}
