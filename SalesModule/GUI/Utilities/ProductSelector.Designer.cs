namespace SalesModule
{
    partial class ProductSelector
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
            this.txtVal = new System.Windows.Forms.TextBox();
            this.DDLTypes = new System.Windows.Forms.ComboBox();
            this.DGVProducts = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pluno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddCommand = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGVProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // txtVal
            // 
            this.txtVal.Location = new System.Drawing.Point(139, 45);
            this.txtVal.Name = "txtVal";
            this.txtVal.Size = new System.Drawing.Size(199, 20);
            this.txtVal.TabIndex = 0;
            // 
            // DDLTypes
            // 
            this.DDLTypes.FormattingEnabled = true;
            this.DDLTypes.Location = new System.Drawing.Point(12, 45);
            this.DDLTypes.Name = "DDLTypes";
            this.DDLTypes.Size = new System.Drawing.Size(121, 21);
            this.DDLTypes.TabIndex = 1;
            // 
            // DGVProducts
            // 
            this.DGVProducts.AllowUserToAddRows = false;
            this.DGVProducts.AllowUserToDeleteRows = false;
            this.DGVProducts.AllowUserToResizeColumns = false;
            this.DGVProducts.AllowUserToResizeRows = false;
            this.DGVProducts.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pluno,
            this.pname,
            this.barcode,
            this.AddCommand});
            this.DGVProducts.Location = new System.Drawing.Point(12, 83);
            this.DGVProducts.MultiSelect = false;
            this.DGVProducts.Name = "DGVProducts";
            this.DGVProducts.ReadOnly = true;
            this.DGVProducts.RowHeadersVisible = false;
            this.DGVProducts.Size = new System.Drawing.Size(550, 237);
            this.DGVProducts.TabIndex = 2;
            this.DGVProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVProducts_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "אנא בחר מוצר:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "חפש";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pluno
            // 
            this.pluno.DataPropertyName = "pluno";
            this.pluno.Frozen = true;
            this.pluno.HeaderText = "מק\"ט";
            this.pluno.Name = "pluno";
            this.pluno.ReadOnly = true;
            this.pluno.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pluno.Width = 75;
            // 
            // pname
            // 
            this.pname.DataPropertyName = "pname";
            this.pname.Frozen = true;
            this.pname.HeaderText = "שם המוצר";
            this.pname.Name = "pname";
            this.pname.ReadOnly = true;
            this.pname.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pname.Width = 250;
            // 
            // barcode
            // 
            this.barcode.DataPropertyName = "barcode";
            this.barcode.Frozen = true;
            this.barcode.HeaderText = "ברקוד";
            this.barcode.Name = "barcode";
            this.barcode.ReadOnly = true;
            this.barcode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // AddCommand
            // 
            this.AddCommand.Frozen = true;
            this.AddCommand.HeaderText = "";
            this.AddCommand.Name = "AddCommand";
            this.AddCommand.ReadOnly = true;
            this.AddCommand.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AddCommand.Text = "הוסף";
            this.AddCommand.UseColumnTextForButtonValue = true;
            this.AddCommand.Width = 80;
            // 
            // ProductSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 332);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DGVProducts);
            this.Controls.Add(this.DDLTypes);
            this.Controls.Add(this.txtVal);
            this.Name = "ProductSelector";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "בחר מוצר";
            this.Load += new System.EventHandler(this.ProductSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVal;
        private System.Windows.Forms.ComboBox DDLTypes;
        private System.Windows.Forms.DataGridView DGVProducts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pluno;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcode;
        private System.Windows.Forms.DataGridViewButtonColumn AddCommand;
    }
}