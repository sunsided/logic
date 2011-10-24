namespace LogicGui
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxEquation = new System.Windows.Forms.GroupBox();
            this.textBoxEquation = new System.Windows.Forms.TextBox();
            this.buttonParse = new System.Windows.Forms.Button();
            this.groupBoxTerms = new System.Windows.Forms.GroupBox();
            this.checkedListBoxTerms = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxEquation.SuspendLayout();
            this.groupBoxTerms.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxEquation
            // 
            this.groupBoxEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxEquation.Controls.Add(this.buttonParse);
            this.groupBoxEquation.Controls.Add(this.textBoxEquation);
            this.groupBoxEquation.Location = new System.Drawing.Point(12, 12);
            this.groupBoxEquation.Name = "groupBoxEquation";
            this.groupBoxEquation.Size = new System.Drawing.Size(446, 74);
            this.groupBoxEquation.TabIndex = 0;
            this.groupBoxEquation.TabStop = false;
            this.groupBoxEquation.Text = "Gleichung";
            // 
            // textBoxEquation
            // 
            this.textBoxEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEquation.Location = new System.Drawing.Point(6, 19);
            this.textBoxEquation.Multiline = true;
            this.textBoxEquation.Name = "textBoxEquation";
            this.textBoxEquation.Size = new System.Drawing.Size(353, 49);
            this.textBoxEquation.TabIndex = 0;
            this.textBoxEquation.Text = "(a or b) and (a or not c)";
            // 
            // buttonParse
            // 
            this.buttonParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonParse.Location = new System.Drawing.Point(365, 19);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(75, 23);
            this.buttonParse.TabIndex = 1;
            this.buttonParse.Text = "&Auswerten";
            this.buttonParse.UseVisualStyleBackColor = true;
            // 
            // groupBoxTerms
            // 
            this.groupBoxTerms.Controls.Add(this.checkedListBoxTerms);
            this.groupBoxTerms.Enabled = false;
            this.groupBoxTerms.Location = new System.Drawing.Point(12, 92);
            this.groupBoxTerms.Name = "groupBoxTerms";
            this.groupBoxTerms.Size = new System.Drawing.Size(235, 171);
            this.groupBoxTerms.TabIndex = 1;
            this.groupBoxTerms.TabStop = false;
            this.groupBoxTerms.Text = "Terme";
            // 
            // checkedListBoxTerms
            // 
            this.checkedListBoxTerms.FormattingEnabled = true;
            this.checkedListBoxTerms.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxTerms.Name = "checkedListBoxTerms";
            this.checkedListBoxTerms.Size = new System.Drawing.Size(223, 139);
            this.checkedListBoxTerms.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(253, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 171);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auswertung";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 275);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxTerms);
            this.Controls.Add(this.groupBoxEquation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Logic GUI";
            this.groupBoxEquation.ResumeLayout(false);
            this.groupBoxEquation.PerformLayout();
            this.groupBoxTerms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEquation;
        private System.Windows.Forms.Button buttonParse;
        private System.Windows.Forms.TextBox textBoxEquation;
        private System.Windows.Forms.GroupBox groupBoxTerms;
        private System.Windows.Forms.CheckedListBox checkedListBoxTerms;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

