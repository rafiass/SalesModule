namespace SalesModule.GUI
{
    partial class SingularBuyAndGet
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
            this.label2 = new System.Windows.Forms.Label();
            this.num_buy = new SalesModule.GUI.TouchableNumeric();
            this.num_get = new SalesModule.GUI.TouchableNumeric();
            this.btn_commit = new System.Windows.Forms.Button();
            this.btn_prop = new System.Windows.Forms.Button();
            this.find_get = new SalesModule.GUI.ProductFinder();
            this.find_buy = new SalesModule.GUI.ProductFinder();
            this.btn_cncl = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_buy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_get)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(71, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 24);
            this.label6.TabIndex = 42;
            this.label6.Text = "מבצע \'קנה וקבל\'";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "קנה פריט";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "קבל פריט";
            // 
            // num_buy
            // 
            this.num_buy.DecimalPlaces = 1;
            this.num_buy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_buy.Location = new System.Drawing.Point(146, 47);
            this.num_buy.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_buy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_buy.Name = "num_buy";
            this.num_buy.Size = new System.Drawing.Size(100, 20);
            this.num_buy.TabIndex = 46;
            this.num_buy.Title = "הכנס כמות";
            this.num_buy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // num_get
            // 
            this.num_get.DecimalPlaces = 1;
            this.num_get.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_get.Location = new System.Drawing.Point(146, 105);
            this.num_get.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_get.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_get.Name = "num_get";
            this.num_get.Size = new System.Drawing.Size(100, 20);
            this.num_get.TabIndex = 47;
            this.num_get.Title = "הכנס כמות";
            this.num_get.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_commit
            // 
            this.btn_commit.Location = new System.Drawing.Point(171, 170);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(75, 44);
            this.btn_commit.TabIndex = 48;
            this.btn_commit.Text = "בצע";
            this.btn_commit.UseVisualStyleBackColor = true;
            this.btn_commit.Click += new System.EventHandler(this.btn_commit_Click);
            // 
            // btn_prop
            // 
            this.btn_prop.Location = new System.Drawing.Point(90, 170);
            this.btn_prop.Name = "btn_prop";
            this.btn_prop.Size = new System.Drawing.Size(75, 44);
            this.btn_prop.TabIndex = 50;
            this.btn_prop.Text = "מאפיינים...";
            this.btn_prop.UseVisualStyleBackColor = true;
            this.btn_prop.Click += new System.EventHandler(this.btn_prop_Click);
            // 
            // find_get
            // 
            this.find_get.Location = new System.Drawing.Point(31, 131);
            this.find_get.Name = "find_get";
            this.find_get.SelectedProduct = null;
            this.find_get.Size = new System.Drawing.Size(215, 20);
            this.find_get.TabIndex = 52;
            // 
            // find_buy
            // 
            this.find_buy.Location = new System.Drawing.Point(31, 71);
            this.find_buy.Name = "find_buy";
            this.find_buy.SelectedProduct = null;
            this.find_buy.Size = new System.Drawing.Size(215, 20);
            this.find_buy.TabIndex = 51;
            // 
            // btn_cncl
            // 
            this.btn_cncl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cncl.Location = new System.Drawing.Point(9, 170);
            this.btn_cncl.Name = "btn_cncl";
            this.btn_cncl.Size = new System.Drawing.Size(75, 44);
            this.btn_cncl.TabIndex = 53;
            this.btn_cncl.Text = "ביטול";
            this.btn_cncl.UseVisualStyleBackColor = true;
            this.btn_cncl.Click += new System.EventHandler(this.btn_cncl_Click);
            // 
            // SingularBuyAndGet
            // 
            this.AcceptButton = this.btn_commit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cncl;
            this.ClientSize = new System.Drawing.Size(258, 226);
            this.Controls.Add(this.btn_cncl);
            this.Controls.Add(this.find_get);
            this.Controls.Add(this.find_buy);
            this.Controls.Add(this.btn_prop);
            this.Controls.Add(this.btn_commit);
            this.Controls.Add(this.num_get);
            this.Controls.Add(this.num_buy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SingularBuyAndGet";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "קנה וקבל";
            ((System.ComponentModel.ISupportInitialize)(this.num_buy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_get)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TouchableNumeric num_buy;
        private TouchableNumeric num_get;
        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Button btn_prop;
        private ProductFinder find_buy;
        private ProductFinder find_get;
        private System.Windows.Forms.Button btn_cncl;
    }
}