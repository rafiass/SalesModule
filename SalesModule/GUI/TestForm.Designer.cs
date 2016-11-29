namespace SalesModule.GUI
{
    partial class TestForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.touchableNumeric1 = new SalesModule.GUI.TouchableNumeric();
            this.num_test = new SalesModule.GUI.TouchableNumeric();
            ((System.ComponentModel.ISupportInitialize)(this.touchableNumeric1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_test)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(82, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // touchableNumeric1
            // 
            this.touchableNumeric1.DecimalPlaces = 2;
            this.touchableNumeric1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.touchableNumeric1.Location = new System.Drawing.Point(92, 78);
            this.touchableNumeric1.Name = "touchableNumeric1";
            this.touchableNumeric1.Size = new System.Drawing.Size(120, 20);
            this.touchableNumeric1.TabIndex = 4;
            this.touchableNumeric1.Title = "הכנס ערך";
            // 
            // num_test
            // 
            this.num_test.DecimalPlaces = 2;
            this.num_test.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.num_test.Location = new System.Drawing.Point(92, 104);
            this.num_test.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_test.Name = "num_test";
            this.num_test.Size = new System.Drawing.Size(120, 20);
            this.num_test.TabIndex = 3;
            this.num_test.Title = "הכנס ערך";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.touchableNumeric1);
            this.Controls.Add(this.num_test);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "TestForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "TestForm";
            ((System.ComponentModel.ISupportInitialize)(this.touchableNumeric1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_test)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TouchableNumeric num_test;
        private TouchableNumeric touchableNumeric1;

    }
}