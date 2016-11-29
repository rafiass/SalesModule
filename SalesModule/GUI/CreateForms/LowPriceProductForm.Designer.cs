namespace SalesModule.GUI
{
    partial class LowPriceProductForm
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
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.check_gift = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.num_max = new SalesModule.GUI.TouchableNumeric();
            this.btn_commit = new System.Windows.Forms.Button();
            this.btn_prop = new System.Windows.Forms.Button();
            this.check_amount = new System.Windows.Forms.CheckBox();
            this.num_amount = new SalesModule.GUI.TouchableNumeric();
            this.check_max = new System.Windows.Forms.CheckBox();
            this.find_gift = new SalesModule.GUI.ProductFinder();
            this.find_buy = new SalesModule.GUI.ProductFinder();
            this.discCntrl = new SalesModule.GUI.DiscountControl();
            this.btn_cncl = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_amount)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(82, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 24);
            this.label6.TabIndex = 42;
            this.label6.Text = "מוצר מוזל";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "בחר מוצר";
            // 
            // check_gift
            // 
            this.check_gift.AutoSize = true;
            this.check_gift.Location = new System.Drawing.Point(15, 258);
            this.check_gift.Name = "check_gift";
            this.check_gift.Size = new System.Drawing.Size(119, 17);
            this.check_gift.TabIndex = 60;
            this.check_gift.Text = "הוסף מוצר במתנה:";
            this.check_gift.UseVisualStyleBackColor = true;
            this.check_gift.CheckedChanged += new System.EventHandler(this.check_gift_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(35, 280);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "בחר מתנה";
            // 
            // num_max
            // 
            this.num_max.DecimalPlaces = 1;
            this.num_max.Enabled = false;
            this.num_max.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_max.Location = new System.Drawing.Point(126, 231);
            this.num_max.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_max.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_max.Name = "num_max";
            this.num_max.Size = new System.Drawing.Size(64, 20);
            this.num_max.TabIndex = 64;
            this.num_max.Title = "הכנס מספר קבוצות";
            this.num_max.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_commit
            // 
            this.btn_commit.Location = new System.Drawing.Point(182, 311);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(75, 45);
            this.btn_commit.TabIndex = 65;
            this.btn_commit.Text = "בצע";
            this.btn_commit.UseVisualStyleBackColor = true;
            this.btn_commit.Click += new System.EventHandler(this.btn_commit_Click);
            // 
            // btn_prop
            // 
            this.btn_prop.Location = new System.Drawing.Point(96, 311);
            this.btn_prop.Name = "btn_prop";
            this.btn_prop.Size = new System.Drawing.Size(75, 45);
            this.btn_prop.TabIndex = 66;
            this.btn_prop.Text = "מאפיינים...";
            this.btn_prop.UseVisualStyleBackColor = true;
            this.btn_prop.Click += new System.EventHandler(this.btn_prop_Click);
            // 
            // check_amount
            // 
            this.check_amount.AutoSize = true;
            this.check_amount.Location = new System.Drawing.Point(15, 209);
            this.check_amount.Name = "check_amount";
            this.check_amount.Size = new System.Drawing.Size(103, 17);
            this.check_amount.TabIndex = 73;
            this.check_amount.Text = "מבצע על כמות:";
            this.check_amount.UseVisualStyleBackColor = true;
            this.check_amount.CheckedChanged += new System.EventHandler(this.check_amount_CheckedChanged);
            // 
            // num_amount
            // 
            this.num_amount.DecimalPlaces = 1;
            this.num_amount.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_amount.Location = new System.Drawing.Point(126, 207);
            this.num_amount.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_amount.Name = "num_amount";
            this.num_amount.Size = new System.Drawing.Size(64, 20);
            this.num_amount.TabIndex = 72;
            this.num_amount.Title = "הכנס כמות";
            this.num_amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // check_max
            // 
            this.check_max.AutoSize = true;
            this.check_max.Location = new System.Drawing.Point(15, 233);
            this.check_max.Name = "check_max";
            this.check_max.Size = new System.Drawing.Size(104, 17);
            this.check_max.TabIndex = 74;
            this.check_max.Text = "הגבלת קבוצות:";
            this.check_max.UseVisualStyleBackColor = true;
            this.check_max.CheckedChanged += new System.EventHandler(this.check_max_CheckedChanged);
            // 
            // find_gift
            // 
            this.find_gift.Enabled = false;
            this.find_gift.Location = new System.Drawing.Point(96, 277);
            this.find_gift.Name = "find_gift";
            this.find_gift.SelectedProduct = null;
            this.find_gift.Size = new System.Drawing.Size(158, 20);
            this.find_gift.TabIndex = 61;
            // 
            // find_buy
            // 
            this.find_buy.Location = new System.Drawing.Point(73, 46);
            this.find_buy.Name = "find_buy";
            this.find_buy.SelectedProduct = null;
            this.find_buy.Size = new System.Drawing.Size(186, 20);
            this.find_buy.TabIndex = 43;
            // 
            // discCntrl
            // 
            this.discCntrl.Location = new System.Drawing.Point(17, 75);
            this.discCntrl.Name = "discCntrl";
            this.discCntrl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.discCntrl.Size = new System.Drawing.Size(236, 128);
            this.discCntrl.TabIndex = 75;
            // 
            // btn_cncl
            // 
            this.btn_cncl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cncl.Location = new System.Drawing.Point(12, 311);
            this.btn_cncl.Name = "btn_cncl";
            this.btn_cncl.Size = new System.Drawing.Size(75, 45);
            this.btn_cncl.TabIndex = 76;
            this.btn_cncl.Text = "ביטול";
            this.btn_cncl.UseVisualStyleBackColor = true;
            this.btn_cncl.Click += new System.EventHandler(this.btn_cncl_Click);
            // 
            // LowPriceProductForm
            // 
            this.AcceptButton = this.btn_commit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cncl;
            this.ClientSize = new System.Drawing.Size(272, 367);
            this.Controls.Add(this.btn_cncl);
            this.Controls.Add(this.discCntrl);
            this.Controls.Add(this.check_max);
            this.Controls.Add(this.check_amount);
            this.Controls.Add(this.num_amount);
            this.Controls.Add(this.num_max);
            this.Controls.Add(this.btn_prop);
            this.Controls.Add(this.btn_commit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.find_gift);
            this.Controls.Add(this.check_gift);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.find_buy);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LowPriceProductForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "מוצר מוזל";
            ((System.ComponentModel.ISupportInitialize)(this.num_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_amount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private ProductFinder find_buy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox check_gift;
        private System.Windows.Forms.Label label4;
        private ProductFinder find_gift;
        private TouchableNumeric num_max;
        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Button btn_prop;
        private System.Windows.Forms.CheckBox check_amount;
        private TouchableNumeric num_amount;
        private System.Windows.Forms.CheckBox check_max;
        private DiscountControl discCntrl;
        private System.Windows.Forms.Button btn_cncl;
    }
}