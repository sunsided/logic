using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Logic;
using Logic.Nodes;

namespace LogicGui
{
    public partial class LogicForm : Form
    {
        /// <summary>
        /// Der geparste Baum
        /// </summary>
        private TokenTree _parsedTree;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Forms.Form"/> class.
        /// </summary>
        /// <remarks></remarks>
        public LogicForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the buttonParse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ButtonParseClick(object sender, EventArgs e)
        {
            Parse(textBoxEquation.Text.Trim());
        }

        /// <summary>
        /// Parst die Eingabe
        /// </summary>
        /// <param name="equation"></param>
        private void Parse(string equation)
        {
            groupBoxTerms.Enabled = false;
            _parsedTree = LogicParser.Parse(equation);
            _parsedTree.Clear();

            IList<string> terms = _parsedTree.Terms;
            checkedListBoxTerms.Items.Clear();
            for (int r = 0; r < terms.Count; ++r)
            {
                checkedListBoxTerms.Items.Add(terms[r], false);
            }
            groupBoxTerms.Enabled = true;
        }

        /// <summary>
        /// Handles the ItemCheck event of the checkedListBoxTerms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemCheckEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void CheckedListBoxTermsItemCheck(object sender, ItemCheckEventArgs e)
        {
            string term = (string) checkedListBoxTerms.SelectedItem;
            bool value = e.NewValue == CheckState.Checked;

            // Auswerten
            _parsedTree[term] = value;
            bool result = _parsedTree.Evaluate();

            // Huiuiui
            labelResult.Text = result.ToString();
        }
    }
}
