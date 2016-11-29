namespace SalesTester
{
    partial class CreateSale
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
            this.salesGUI1 = new SalesModule.SalesGUI();
            this.SuspendLayout();
            // 
            // salesGUI1
            // 
            this.salesGUI1.BackColor = System.Drawing.SystemColors.Control;
            this.salesGUI1.BackgroundColor = -2147483633;
            this.salesGUI1.Enabled = false;
            this.salesGUI1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.salesGUI1.ForegroundColor = -2147483630;
            this.salesGUI1.Location = new System.Drawing.Point(2, 2);
            this.salesGUI1.Name = "salesGUI1";
            this.salesGUI1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.salesGUI1.Size = new System.Drawing.Size(377, 232);
            this.salesGUI1.TabIndex = 0;
            // 
            // CreateSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 235);
            this.Controls.Add(this.salesGUI1);
            this.Name = "CreateSale";
            this.Text = "CreateSale";
            this.ResumeLayout(false);

        }

        #endregion

        private SalesModule.SalesGUI salesGUI1;

    }
}