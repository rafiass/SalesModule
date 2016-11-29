namespace SalesTester
{
    partial class CashierAutomaticTester
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
            this.lstbx_tests = new System.Windows.Forms.ListBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.txt_console = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_all = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstbx_tests
            // 
            this.lstbx_tests.FormattingEnabled = true;
            this.lstbx_tests.Location = new System.Drawing.Point(12, 63);
            this.lstbx_tests.Name = "lstbx_tests";
            this.lstbx_tests.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lstbx_tests.Size = new System.Drawing.Size(165, 251);
            this.lstbx_tests.TabIndex = 0;
            this.lstbx_tests.SelectedIndexChanged += new System.EventHandler(this.lstbx_tests_SelectedIndexChanged);
            this.lstbx_tests.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstbx_tests_MouseDoubleClick);
            // 
            // btn_run
            // 
            this.btn_run.Enabled = false;
            this.btn_run.Location = new System.Drawing.Point(12, 322);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(165, 40);
            this.btn_run.TabIndex = 1;
            this.btn_run.Text = "הפעל נבחר";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // txt_console
            // 
            this.txt_console.Location = new System.Drawing.Point(183, 63);
            this.txt_console.Multiline = true;
            this.txt_console.Name = "txt_console";
            this.txt_console.ReadOnly = true;
            this.txt_console.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_console.Size = new System.Drawing.Size(444, 391);
            this.txt_console.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(224, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "בדיקות אוטומטיות";
            // 
            // btn_all
            // 
            this.btn_all.Location = new System.Drawing.Point(12, 368);
            this.btn_all.Name = "btn_all";
            this.btn_all.Size = new System.Drawing.Size(165, 40);
            this.btn_all.TabIndex = 4;
            this.btn_all.Text = "הפעל הכל";
            this.btn_all.UseVisualStyleBackColor = true;
            this.btn_all.Click += new System.EventHandler(this.btn_all_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(12, 414);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(165, 40);
            this.btn_reset.TabIndex = 5;
            this.btn_reset.Text = "איפוס";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // CashierAutomaticTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 466);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_all);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_console);
            this.Controls.Add(this.btn_run);
            this.Controls.Add(this.lstbx_tests);
            this.Name = "CashierAutomaticTester";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "בדיקות מנוע אוטומטיות";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstbx_tests;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.TextBox txt_console;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_all;
        private System.Windows.Forms.Button btn_reset;
    }
}