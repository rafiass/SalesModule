namespace SalesModule.GUI
{
    partial class VIPAssociation
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
            this.DGVassoc = new System.Windows.Forms.DataGridView();
            this.vipa_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vipa_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vipa_cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DGVgroup = new System.Windows.Forms.DataGridView();
            this.vipg_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vipg_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vipg_cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DGVsingle = new System.Windows.Forms.DataGridView();
            this.vips_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vips_cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblAssoc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGVassoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVgroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVsingle)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVassoc
            // 
            this.DGVassoc.AllowUserToAddRows = false;
            this.DGVassoc.AllowUserToDeleteRows = false;
            this.DGVassoc.AllowUserToResizeColumns = false;
            this.DGVassoc.AllowUserToResizeRows = false;
            this.DGVassoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVassoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vipa_name,
            this.vipa_type,
            this.vipa_cmd});
            this.DGVassoc.Location = new System.Drawing.Point(282, 68);
            this.DGVassoc.Name = "DGVassoc";
            this.DGVassoc.ReadOnly = true;
            this.DGVassoc.RowHeadersVisible = false;
            this.DGVassoc.Size = new System.Drawing.Size(258, 335);
            this.DGVassoc.TabIndex = 0;
            this.DGVassoc.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVassoc_CellContentClick);
            this.DGVassoc.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGVassoc_CellFormatting);
            // 
            // vipa_name
            // 
            this.vipa_name.DataPropertyName = "name";
            this.vipa_name.Frozen = true;
            this.vipa_name.HeaderText = "שם";
            this.vipa_name.Name = "vipa_name";
            this.vipa_name.ReadOnly = true;
            this.vipa_name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vipa_name.Width = 120;
            // 
            // vipa_type
            // 
            this.vipa_type.Frozen = true;
            this.vipa_type.HeaderText = "סוג";
            this.vipa_type.Name = "vipa_type";
            this.vipa_type.ReadOnly = true;
            this.vipa_type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vipa_type.Width = 70;
            // 
            // vipa_cmd
            // 
            this.vipa_cmd.Frozen = true;
            this.vipa_cmd.HeaderText = "";
            this.vipa_cmd.Name = "vipa_cmd";
            this.vipa_cmd.ReadOnly = true;
            this.vipa_cmd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vipa_cmd.Text = "X";
            this.vipa_cmd.UseColumnTextForButtonValue = true;
            this.vipa_cmd.Width = 40;
            // 
            // DGVgroup
            // 
            this.DGVgroup.AllowUserToAddRows = false;
            this.DGVgroup.AllowUserToDeleteRows = false;
            this.DGVgroup.AllowUserToResizeColumns = false;
            this.DGVgroup.AllowUserToResizeRows = false;
            this.DGVgroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVgroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vipg_name,
            this.vipg_count,
            this.vipg_cmd});
            this.DGVgroup.Location = new System.Drawing.Point(12, 248);
            this.DGVgroup.Name = "DGVgroup";
            this.DGVgroup.ReadOnly = true;
            this.DGVgroup.RowHeadersVisible = false;
            this.DGVgroup.Size = new System.Drawing.Size(258, 155);
            this.DGVgroup.TabIndex = 1;
            this.DGVgroup.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVgroup_CellContentClick);
            // 
            // vipg_name
            // 
            this.vipg_name.DataPropertyName = "gname";
            this.vipg_name.Frozen = true;
            this.vipg_name.HeaderText = "שם המועדון";
            this.vipg_name.Name = "vipg_name";
            this.vipg_name.ReadOnly = true;
            this.vipg_name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vipg_name.Width = 130;
            // 
            // vipg_count
            // 
            this.vipg_count.DataPropertyName = "membersCount";
            this.vipg_count.Frozen = true;
            this.vipg_count.HeaderText = "משתתפים";
            this.vipg_count.Name = "vipg_count";
            this.vipg_count.ReadOnly = true;
            this.vipg_count.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vipg_count.Width = 70;
            // 
            // vipg_cmd
            // 
            this.vipg_cmd.Frozen = true;
            this.vipg_cmd.HeaderText = "";
            this.vipg_cmd.Name = "vipg_cmd";
            this.vipg_cmd.ReadOnly = true;
            this.vipg_cmd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vipg_cmd.Text = "->";
            this.vipg_cmd.UseColumnTextForButtonValue = true;
            this.vipg_cmd.Width = 40;
            // 
            // DGVsingle
            // 
            this.DGVsingle.AllowUserToAddRows = false;
            this.DGVsingle.AllowUserToDeleteRows = false;
            this.DGVsingle.AllowUserToResizeColumns = false;
            this.DGVsingle.AllowUserToResizeRows = false;
            this.DGVsingle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVsingle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vips_name,
            this.vips_cmd});
            this.DGVsingle.Location = new System.Drawing.Point(12, 68);
            this.DGVsingle.Name = "DGVsingle";
            this.DGVsingle.ReadOnly = true;
            this.DGVsingle.RowHeadersVisible = false;
            this.DGVsingle.Size = new System.Drawing.Size(258, 155);
            this.DGVsingle.TabIndex = 2;
            this.DGVsingle.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVsingle_CellContentClick);
            // 
            // vips_name
            // 
            this.vips_name.DataPropertyName = "vname";
            this.vips_name.Frozen = true;
            this.vips_name.HeaderText = "שם הלקוח";
            this.vips_name.Name = "vips_name";
            this.vips_name.ReadOnly = true;
            this.vips_name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vips_name.Width = 150;
            // 
            // vips_cmd
            // 
            this.vips_cmd.Frozen = true;
            this.vips_cmd.HeaderText = "";
            this.vips_cmd.Name = "vips_cmd";
            this.vips_cmd.ReadOnly = true;
            this.vips_cmd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vips_cmd.Text = "->";
            this.vips_cmd.UseColumnTextForButtonValue = true;
            this.vips_cmd.Width = 40;
            // 
            // lblAssoc
            // 
            this.lblAssoc.AutoSize = true;
            this.lblAssoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblAssoc.Location = new System.Drawing.Point(367, 47);
            this.lblAssoc.Name = "lblAssoc";
            this.lblAssoc.Size = new System.Drawing.Size(103, 18);
            this.lblAssoc.TabIndex = 3;
            this.lblAssoc.Text = "לקוחות מורשים";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(85, 227);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "מועדוני לקוחות";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(85, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "לקוחות פרטיים";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblHeader.Location = new System.Drawing.Point(8, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(251, 20);
            this.lblHeader.TabIndex = 6;
            this.lblHeader.Text = "אנא בחרו אילו לקוחות יהנו מהמבצע:";
            // 
            // VIPAssociation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 415);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAssoc);
            this.Controls.Add(this.DGVsingle);
            this.Controls.Add(this.DGVgroup);
            this.Controls.Add(this.DGVassoc);
            this.Name = "VIPAssociation";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.Text = "VIPAssociation";
            ((System.ComponentModel.ISupportInitialize)(this.DGVassoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVgroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVsingle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVassoc;
        private System.Windows.Forms.DataGridView DGVgroup;
        private System.Windows.Forms.DataGridView DGVsingle;
        private System.Windows.Forms.Label lblAssoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn vipa_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn vipa_type;
        private System.Windows.Forms.DataGridViewButtonColumn vipa_cmd;
        private System.Windows.Forms.DataGridViewTextBoxColumn vipg_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn vipg_count;
        private System.Windows.Forms.DataGridViewButtonColumn vipg_cmd;
        private System.Windows.Forms.DataGridViewTextBoxColumn vips_name;
        private System.Windows.Forms.DataGridViewButtonColumn vips_cmd;
    }
}