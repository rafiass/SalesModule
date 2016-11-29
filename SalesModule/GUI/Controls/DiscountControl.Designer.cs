namespace SalesModule.GUI
{
    partial class DiscountControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.num_percentage = new SalesModule.GUI.TouchableNumeric();
            this.rad_percentage = new System.Windows.Forms.RadioButton();
            this.num_disc = new SalesModule.GUI.TouchableNumeric();
            this.num_fix = new SalesModule.GUI.TouchableNumeric();
            this.lbl_status = new System.Windows.Forms.Label();
            this.rad_fix_price = new System.Windows.Forms.RadioButton();
            this.rad_fix_disc = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_percentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_disc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fix)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.num_percentage);
            this.groupBox1.Controls.Add(this.rad_percentage);
            this.groupBox1.Controls.Add(this.num_disc);
            this.groupBox1.Controls.Add(this.num_fix);
            this.groupBox1.Controls.Add(this.lbl_status);
            this.groupBox1.Controls.Add(this.rad_fix_price);
            this.groupBox1.Controls.Add(this.rad_fix_disc);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 121);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "קביעת הנחה";
            // 
            // num_percentage
            // 
            this.num_percentage.DecimalPlaces = 2;
            this.num_percentage.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_percentage.Location = new System.Drawing.Point(10, 68);
            this.num_percentage.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_percentage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_percentage.Name = "num_percentage";
            this.num_percentage.Size = new System.Drawing.Size(76, 20);
            this.num_percentage.TabIndex = 69;
            this.num_percentage.Title = "הכנס הנחה באחוזים";
            this.num_percentage.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // rad_percentage
            // 
            this.rad_percentage.AutoSize = true;
            this.rad_percentage.Location = new System.Drawing.Point(93, 68);
            this.rad_percentage.Name = "rad_percentage";
            this.rad_percentage.Size = new System.Drawing.Size(123, 17);
            this.rad_percentage.TabIndex = 68;
            this.rad_percentage.Text = "קבע הנחה של       %";
            this.rad_percentage.UseVisualStyleBackColor = true;
            this.rad_percentage.CheckedChanged += new System.EventHandler(this.rad_percentage_CheckedChanged);
            // 
            // num_disc
            // 
            this.num_disc.DecimalPlaces = 2;
            this.num_disc.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_disc.Location = new System.Drawing.Point(10, 45);
            this.num_disc.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_disc.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_disc.Name = "num_disc";
            this.num_disc.Size = new System.Drawing.Size(76, 20);
            this.num_disc.TabIndex = 7;
            this.num_disc.Title = "הכנס הנחה קבועה";
            this.num_disc.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // num_fix
            // 
            this.num_fix.DecimalPlaces = 2;
            this.num_fix.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_fix.Location = new System.Drawing.Point(10, 22);
            this.num_fix.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_fix.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_fix.Name = "num_fix";
            this.num_fix.Size = new System.Drawing.Size(76, 20);
            this.num_fix.TabIndex = 6;
            this.num_fix.Title = "הכנס מחיר חדש";
            this.num_fix.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lbl_status
            // 
            this.lbl_status.Enabled = false;
            this.lbl_status.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_status.Location = new System.Drawing.Point(10, 91);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(200, 13);
            this.lbl_status.TabIndex = 67;
            // 
            // rad_fix_price
            // 
            this.rad_fix_price.AutoSize = true;
            this.rad_fix_price.Location = new System.Drawing.Point(92, 22);
            this.rad_fix_price.Name = "rad_fix_price";
            this.rad_fix_price.Size = new System.Drawing.Size(124, 17);
            this.rad_fix_price.TabIndex = 54;
            this.rad_fix_price.Text = "קבע מחיר חדש     ₪";
            this.rad_fix_price.UseVisualStyleBackColor = true;
            this.rad_fix_price.CheckedChanged += new System.EventHandler(this.rad_fix_price_CheckedChanged);
            // 
            // rad_fix_disc
            // 
            this.rad_fix_disc.AutoSize = true;
            this.rad_fix_disc.Location = new System.Drawing.Point(92, 45);
            this.rad_fix_disc.Name = "rad_fix_disc";
            this.rad_fix_disc.Size = new System.Drawing.Size(124, 17);
            this.rad_fix_disc.TabIndex = 58;
            this.rad_fix_disc.Text = "קבע הנחה של       ₪";
            this.rad_fix_disc.UseVisualStyleBackColor = true;
            this.rad_fix_disc.CheckedChanged += new System.EventHandler(this.rad_fix_disc_CheckedChanged);
            // 
            // DiscountControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "DiscountControl";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(236, 128);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_percentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_disc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_fix)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private TouchableNumeric num_percentage;
        private System.Windows.Forms.RadioButton rad_percentage;
        private TouchableNumeric num_disc;
        private TouchableNumeric num_fix;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.RadioButton rad_fix_price;
        private System.Windows.Forms.RadioButton rad_fix_disc;
    }
}
