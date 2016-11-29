namespace SalesModule.GUI
{
    partial class BundleAdvancedForm
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
            this.dgv_bundle = new System.Windows.Forms.DataGridView();
            this.delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bundle_pname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bundle_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_commit = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_prop = new System.Windows.Forms.Button();
            this.num_price = new SalesModule.GUI.TouchableNumeric();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bundle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_price)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(105, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 24);
            this.label6.TabIndex = 38;
            this.label6.Text = "חבילת מוצרים במבצע";
            // 
            // dgv_bundle
            // 
            this.dgv_bundle.AllowUserToAddRows = false;
            this.dgv_bundle.AllowUserToDeleteRows = false;
            this.dgv_bundle.AllowUserToResizeColumns = false;
            this.dgv_bundle.AllowUserToResizeRows = false;
            this.dgv_bundle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_bundle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.delete,
            this.bundle_pname,
            this.bundle_amount,
            this.type});
            this.dgv_bundle.Location = new System.Drawing.Point(6, 73);
            this.dgv_bundle.MultiSelect = false;
            this.dgv_bundle.Name = "dgv_bundle";
            this.dgv_bundle.RowHeadersVisible = false;
            this.dgv_bundle.Size = new System.Drawing.Size(400, 194);
            this.dgv_bundle.TabIndex = 41;
            this.dgv_bundle.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_bundle_CellContentClick);
            this.dgv_bundle.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_bundle_CellFormatting);
            this.dgv_bundle.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_bundle_CellValueChanged);
            // 
            // delete
            // 
            this.delete.Frozen = true;
            this.delete.HeaderText = "";
            this.delete.Name = "delete";
            this.delete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.delete.Text = "X";
            this.delete.UseColumnTextForButtonValue = true;
            this.delete.Width = 30;
            // 
            // bundle_pname
            // 
            this.bundle_pname.DataPropertyName = "pname";
            this.bundle_pname.Frozen = true;
            this.bundle_pname.HeaderText = "שם";
            this.bundle_pname.Name = "bundle_pname";
            this.bundle_pname.ReadOnly = true;
            this.bundle_pname.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.bundle_pname.Width = 190;
            // 
            // bundle_amount
            // 
            this.bundle_amount.Frozen = true;
            this.bundle_amount.HeaderText = "כמות";
            this.bundle_amount.Name = "bundle_amount";
            this.bundle_amount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.bundle_amount.Width = 60;
            // 
            // type
            // 
            this.type.DataPropertyName = "type";
            this.type.Frozen = true;
            this.type.HeaderText = "סוג";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.type.Width = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(250, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "מחיר החבילה";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(3, 57);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "הרכב החבילה:";
            // 
            // btn_commit
            // 
            this.btn_commit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_commit.Location = new System.Drawing.Point(331, 273);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(75, 23);
            this.btn_commit.TabIndex = 45;
            this.btn_commit.Text = "בצע";
            this.btn_commit.UseVisualStyleBackColor = true;
            this.btn_commit.Click += new System.EventHandler(this.btn_commit_Click);
            // 
            // btn_add
            // 
            this.btn_add.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_add.Location = new System.Drawing.Point(6, 273);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(130, 23);
            this.btn_add.TabIndex = 46;
            this.btn_add.Text = "הוסף מוצר נוסף";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_prop
            // 
            this.btn_prop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_prop.Location = new System.Drawing.Point(250, 273);
            this.btn_prop.Name = "btn_prop";
            this.btn_prop.Size = new System.Drawing.Size(75, 23);
            this.btn_prop.TabIndex = 47;
            this.btn_prop.Text = "מאפיינים...";
            this.btn_prop.UseVisualStyleBackColor = true;
            this.btn_prop.Click += new System.EventHandler(this.btn_prop_Click);
            // 
            // num_price
            // 
            this.num_price.DecimalPlaces = 2;
            this.num_price.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_price.Location = new System.Drawing.Point(331, 47);
            this.num_price.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_price.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_price.Name = "num_price";
            this.num_price.Size = new System.Drawing.Size(72, 20);
            this.num_price.TabIndex = 48;
            this.num_price.Title = "הכנס מחיר חדש";
            this.num_price.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // BundleAdvancedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 302);
            this.Controls.Add(this.num_price);
            this.Controls.Add(this.btn_prop);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_commit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_bundle);
            this.Controls.Add(this.label6);
            this.Name = "BundleAdvancedForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.Text = "יצירת מבצע חדש על חבילת מוצרים";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bundle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_price)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgv_bundle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_prop;
        private TouchableNumeric num_price;
        private System.Windows.Forms.DataGridViewButtonColumn delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn bundle_pname;
        private System.Windows.Forms.DataGridViewTextBoxColumn bundle_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
    }
}