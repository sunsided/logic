using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Logic.Nodes
{
    /// <summary>
    /// Der Tokenbaum
    /// </summary>
    public sealed class TokenTree
    {
        /// <summary>
        /// Das Wurzelelement
        /// </summary>
        public TokenNode Root { [Pure] get; private set; }

        /// <summary>
        /// Die Terme
        /// </summary>
        private readonly Dictionary<string, IList<TermNode>> _terms = new Dictionary<string, IList<TermNode>>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Die Terme
        /// </summary>
        public IList<string> Terms
        {
            get { return _terms.Keys.ToList(); }
        }

        /// <summary>
        /// Setzt die Werte der Terme
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public bool this[string term]
        {
            get
            {
                if (!_terms.ContainsKey(term)) throw new ArgumentException("term");
                return _terms[term][0].Value;
            }
            set
            {
                if (!_terms.ContainsKey(term)) throw new ArgumentException("term");
                for (int t=0; t<_terms[term].Count; ++t)
                {
                    _terms[term][t].Value = value;
                }
            }
        }

        /// <summary>
        /// Wertet den Baum aus
        /// </summary>
        /// <returns></returns>
        public bool Evaluate()
        {
            return Root.Evaluate();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            foreach (var term in _terms)
            {
                foreach (TermNode t1 in term.Value)
                {
                    t1.Value = false;
                }
            }
        }

        /// <summary>
        /// Setzt den Wert des Terms
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <remarks></remarks>
        public void SetTerm(string term, bool value)
        {
            this[term] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenTree"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <remarks></remarks>
        internal TokenTree(TokenNode node)
        {
            Contract.Requires(node != null);

            Root = node;
            FillTermList(node);
        }

        /// <summary>
        /// Erzeugt die interne Liste der Terme
        /// </summary>
        /// <param name="node">Der Startknoten</param>
        private void FillTermList(TokenNode node)
        {
            Contract.Requires(node != null);

            if (node is TermNode)
            {
                TermNode tn = (TermNode)node;
                if (!_terms.ContainsKey(tn.Name))
                {
                    _terms.Add(tn.Name, new List<TermNode>());
                }
                _terms[tn.Name].Add(tn);
            }
            else
            {
                if (node is UnaryOperatorNode)
                {
                    UnaryOperatorNode un = (UnaryOperatorNode) node;
                    FillTermList(un.Node);
                }
                else if(node is BinaryOperatorNode)
                {
                    BinaryOperatorNode bn = (BinaryOperatorNode) node;
                    FillTermList(bn.LeftNode);
                    FillTermList(bn.RightNode);
                }
            }
        }
    }
}
