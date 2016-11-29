namespace SalesModule.GUI
{
    partial class PCIDAssociation
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
            this.label2 = new System.Windows.Forms.Label();
            this.panel_reg = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_add = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.combo_to = new System.Windows.Forms.ComboBox();
            this.combo_from = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.combo_pcid = new System.Windows.Forms.ComboBox();
            this.DGV_assoc = new System.Windows.Forms.DataGridView();
            this.pEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pdateFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pdateTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pHourFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pHourTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_reg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_assoc)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(187, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "ניהול מבצע בסניפים";
            // 
            // panel_reg
            // 
            this.panel_reg.Controls.Add(this.label6);
            this.panel_reg.Controls.Add(this.btn_add);
            this.panel_reg.Controls.Add(this.label5);
            this.panel_reg.Controls.Add(this.label4);
            this.panel_reg.Controls.Add(this.dateTimePicker2);
            this.panel_reg.Controls.Add(this.combo_to);
            this.panel_reg.Controls.Add(this.combo_from);
            this.panel_reg.Controls.Add(this.label3);
            this.panel_reg.Controls.Add(this.dateTimePicker1);
            this.panel_reg.Controls.Add(this.combo_pcid);
            this.panel_reg.Location = new System.Drawing.Point(12, 62);
            this.panel_reg.Name = "panel_reg";
            this.panel_reg.Size = new System.Drawing.Size(528, 125);
            this.panel_reg.TabIndex = 3;
            this.panel_reg.TabStop = false;
            this.panel_reg.Text = "הוספת סניף חדש";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(276, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "עד";
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(6, 93);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 8;
            this.btn_add.Text = "הוסף";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(439, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "שעות פעילות";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(436, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "טווח תאריכים";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePicker2.Location = new System.Drawing.Point(6, 56);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.RightToLeftLayout = true;
            this.dateTimePicker2.ShowCheckBox = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 5;
            // 
            // combo_to
            // 
            this.combo_to.FormattingEnabled = true;
            this.combo_to.Location = new System.Drawing.Point(149, 95);
            this.combo_to.Name = "combo_to";
            this.combo_to.Size = new System.Drawing.Size(121, 21);
            this.combo_to.TabIndex = 4;
            // 
            // combo_from
            // 
            this.combo_from.FormattingEnabled = true;
            this.combo_from.Location = new System.Drawing.Point(303, 95);
            this.combo_from.Name = "combo_from";
            this.combo_from.Size = new System.Drawing.Size(121, 21);
            this.combo_from.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(455, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "שם הסניף:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePicker1.Location = new System.Drawing.Point(224, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.RightToLeftLayout = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // combo_pcid
            // 
            this.combo_pcid.FormattingEnabled = true;
            this.combo_pcid.Location = new System.Drawing.Point(303, 19);
            this.combo_pcid.Name = "combo_pcid";
            this.combo_pcid.Size = new System.Drawing.Size(121, 21);
            this.combo_pcid.TabIndex = 0;
            // 
            // DGV_assoc
            // 
            this.DGV_assoc.AllowUserToAddRows = false;
            this.DGV_assoc.AllowUserToDeleteRows = false;
            this.DGV_assoc.AllowUserToResizeColumns = false;
            this.DGV_assoc.AllowUserToResizeRows = false;
            this.DGV_assoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_assoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pEnabled,
            this.pname,
            this.pdateFrom,
            this.pdateTo,
            this.pHourFrom,
            this.pHourTo,
            this.cmd});
            this.DGV_assoc.Location = new System.Drawing.Point(12, 215);
            this.DGV_assoc.Name = "DGV_assoc";
            this.DGV_assoc.ReadOnly = true;
            this.DGV_assoc.RowHeadersVisible = false;
            this.DGV_assoc.Size = new System.Drawing.Size(528, 188);
            this.DGV_assoc.TabIndex = 4;
            this.DGV_assoc.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_assoc_CellContentClick);
            // 
            // pEnabled
            // 
            this.pEnabled.DataPropertyName = "isEnabled";
            this.pEnabled.FalseValue = "0";
            this.pEnabled.Frozen = true;
            this.pEnabled.HeaderText = "";
            this.pEnabled.Name = "pEnabled";
            this.pEnabled.ReadOnly = true;
            this.pEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pEnabled.TrueValue = "1";
            this.pEnabled.Width = 30;
            // 
            // pname
            // 
            this.pname.DataPropertyName = "bhname";
            this.pname.Frozen = true;
            this.pname.HeaderText = "שם הסניף";
            this.pname.Name = "pname";
            this.pname.ReadOnly = true;
            this.pname.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // pdateFrom
            // 
            this.pdateFrom.DataPropertyName = "DateFrom";
            this.pdateFrom.Frozen = true;
            this.pdateFrom.HeaderText = "ת. התחלה";
            this.pdateFrom.Name = "pdateFrom";
            this.pdateFrom.ReadOnly = true;
            this.pdateFrom.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pdateFrom.Width = 90;
            // 
            // pdateTo
            // 
            this.pdateTo.DataPropertyName = "DateTo";
            this.pdateTo.Frozen = true;
            this.pdateTo.HeaderText = "ת. סיום";
            this.pdateTo.Name = "pdateTo";
            this.pdateTo.ReadOnly = true;
            this.pdateTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pdateTo.Width = 90;
            // 
            // pHourFrom
            // 
            this.pHourFrom.DataPropertyName = "HourFrom";
            this.pHourFrom.Frozen = true;
            this.pHourFrom.HeaderText = "משעה";
            this.pHourFrom.Name = "pHourFrom";
            this.pHourFrom.ReadOnly = true;
            this.pHourFrom.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pHourFrom.Width = 80;
            // 
            // pHourTo
            // 
            this.pHourTo.DataPropertyName = "HourTo";
            this.pHourTo.Frozen = true;
            this.pHourTo.HeaderText = "עד שעה";
            this.pHourTo.Name = "pHourTo";
            this.pHourTo.ReadOnly = true;
            this.pHourTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pHourTo.Width = 80;
            // 
            // cmd
            // 
            this.cmd.Frozen = true;
            this.cmd.HeaderText = "";
            this.cmd.Name = "cmd";
            this.cmd.ReadOnly = true;
            this.cmd.Text = "X";
            this.cmd.UseColumnTextForButtonValue = true;
            this.cmd.Width = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "סניפים מקושרים";
            // 
            // PCIDAssociation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 415);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DGV_assoc);
            this.Controls.Add(this.panel_reg);
            this.Controls.Add(this.label2);
            this.Name = "PCIDAssociation";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.Text = "PCIDAssociation";
            this.panel_reg.ResumeLayout(false);
            this.panel_reg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_assoc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox panel_reg;
        private System.Windows.Forms.DataGridView DGV_assoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox combo_to;
        private System.Windows.Forms.ComboBox combo_from;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox combo_pcid;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.DataGridViewCheckBoxColumn pEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname;
        private System.Windows.Forms.DataGridViewTextBoxColumn pdateFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn pdateTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn pHourFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn pHourTo;
        private System.Windows.Forms.DataGridViewButtonColumn cmd;
        private System.Windows.Forms.Label label6;
    }
}