namespace SalesModule.GUI
{
    partial class SalesManagement
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
            this.dgv_sales = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vip_cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pcid_cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.edit_cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_sales)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_sales
            // 
            this.dgv_sales.AllowUserToAddRows = false;
            this.dgv_sales.AllowUserToDeleteRows = false;
            this.dgv_sales.AllowUserToResizeColumns = false;
            this.dgv_sales.AllowUserToResizeRows = false;
            this.dgv_sales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_sales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enabled,
            this.id,
            this.title,
            this.ename,
            this.dateCreated,
            this.vip_cmd,
            this.pcid_cmd,
            this.edit_cmd});
            this.dgv_sales.Location = new System.Drawing.Point(12, 38);
            this.dgv_sales.Name = "dgv_sales";
            this.dgv_sales.ReadOnly = true;
            this.dgv_sales.RowHeadersVisible = false;
            this.dgv_sales.Size = new System.Drawing.Size(654, 312);
            this.dgv_sales.TabIndex = 0;
            this.dgv_sales.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_sales_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(240, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "ניהול מבצעים";
            // 
            // enabled
            // 
            this.enabled.DataPropertyName = "isEnabled";
            this.enabled.FalseValue = "0";
            this.enabled.Frozen = true;
            this.enabled.HeaderText = "";
            this.enabled.Name = "enabled";
            this.enabled.ReadOnly = true;
            this.enabled.TrueValue = "1";
            this.enabled.Width = 30;
            // 
            // id
            // 
            this.id.DataPropertyName = "SaleID";
            this.id.Frozen = true;
            this.id.HeaderText = "#";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.id.Width = 20;
            // 
            // title
            // 
            this.title.DataPropertyName = "Title";
            this.title.Frozen = true;
            this.title.HeaderText = "שם המבצע";
            this.title.Name = "title";
            this.title.ReadOnly = true;
            this.title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.title.Width = 230;
            // 
            // ename
            // 
            this.ename.DataPropertyName = "ename";
            this.ename.Frozen = true;
            this.ename.HeaderText = "יוצר המבצע";
            this.ename.Name = "ename";
            this.ename.ReadOnly = true;
            this.ename.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ename.Width = 95;
            // 
            // dateCreated
            // 
            this.dateCreated.DataPropertyName = "DateCreated";
            this.dateCreated.Frozen = true;
            this.dateCreated.HeaderText = "יצירה";
            this.dateCreated.Name = "dateCreated";
            this.dateCreated.ReadOnly = true;
            this.dateCreated.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dateCreated.Width = 80;
            // 
            // vip_cmd
            // 
            this.vip_cmd.Frozen = true;
            this.vip_cmd.HeaderText = "לקוחות";
            this.vip_cmd.Name = "vip_cmd";
            this.vip_cmd.ReadOnly = true;
            this.vip_cmd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vip_cmd.Text = "נהל";
            this.vip_cmd.UseColumnTextForButtonValue = true;
            this.vip_cmd.Width = 55;
            // 
            // pcid_cmd
            // 
            this.pcid_cmd.Frozen = true;
            this.pcid_cmd.HeaderText = "סניפים";
            this.pcid_cmd.Name = "pcid_cmd";
            this.pcid_cmd.ReadOnly = true;
            this.pcid_cmd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pcid_cmd.Text = "נהל";
            this.pcid_cmd.UseColumnTextForButtonValue = true;
            this.pcid_cmd.Width = 55;
            // 
            // edit_cmd
            // 
            this.edit_cmd.Frozen = true;
            this.edit_cmd.HeaderText = "עריכה";
            this.edit_cmd.Name = "edit_cmd";
            this.edit_cmd.ReadOnly = true;
            this.edit_cmd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.edit_cmd.Text = "ערוך";
            this.edit_cmd.UseColumnTextForButtonValue = true;
            this.edit_cmd.Width = 55;
            // 
            // SalesManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 362);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_sales);
            this.Name = "SalesManagement";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.Text = "SalesManagement";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_sales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_sales;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewTextBoxColumn ename;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateCreated;
        private System.Windows.Forms.DataGridViewButtonColumn vip_cmd;
        private System.Windows.Forms.DataGridViewButtonColumn pcid_cmd;
        private System.Windows.Forms.DataGridViewButtonColumn edit_cmd;

    }
}