namespace SalesModule.GUI
{
    partial class SalesPropertiesForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.check_dates = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.check_max = new System.Windows.Forms.CheckBox();
            this.num_min = new SalesModule.GUI.TouchableNumeric();
            this.num_max = new SalesModule.GUI.TouchableNumeric();
            this.num_multiply = new SalesModule.GUI.TouchableNumeric();
            this.num_rec = new SalesModule.GUI.TouchableNumeric();
            this.check_multiply = new System.Windows.Forms.CheckBox();
            this.check_rec = new System.Windows.Forms.CheckBox();
            this.HoverTexts = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.num_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_multiply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_rec)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(78, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "הגדרות מתקדמות";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ערך מינימאלי של הסל";
            this.HoverTexts.SetToolTip(this.label2, "סכום סל הקניות המינימלי עבורו המבצע נכנס לתוקף");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "הגבל כפל מבצעים";
            this.HoverTexts.SetToolTip(this.label3, "כמה פעמים באותה קנייה יכול המבצע להיכנס לתוקף");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "ערך מקסימאלי של הסל";
            this.HoverTexts.SetToolTip(this.label4, "סכום מקסימאלי של סל הקניות שמתחתיו המבצע תקף");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "חזרות עבור כל כפילות";
            this.HoverTexts.SetToolTip(this.label5, "כמה פעמים המבצע יהיה בתוקף עבור כל תנאי התחלה מתאים");
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(179, 267);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 44);
            this.btn_ok.TabIndex = 9;
            this.btn_ok.Text = "אישור";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(12, 267);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 44);
            this.btn_cancel.TabIndex = 10;
            this.btn_cancel.Text = "ביטול";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // check_dates
            // 
            this.check_dates.AutoSize = true;
            this.check_dates.Checked = true;
            this.check_dates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_dates.Location = new System.Drawing.Point(15, 184);
            this.check_dates.Name = "check_dates";
            this.check_dates.Size = new System.Drawing.Size(127, 17);
            this.check_dates.TabIndex = 11;
            this.check_dates.Text = "החל על כל הסניפים";
            this.check_dates.UseVisualStyleBackColor = true;
            this.check_dates.CheckedChanged += new System.EventHandler(this.check_dates_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(43, 207);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.RightToLeftLayout = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 12;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(43, 233);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.RightToLeftLayout = true;
            this.dateTimePicker2.ShowCheckBox = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "מ-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 239);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "עד-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "שם המבצע";
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(84, 37);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(159, 20);
            this.txt_name.TabIndex = 17;
            this.txt_name.Leave += new System.EventHandler(this.txt_name_Leave);
            // 
            // check_max
            // 
            this.check_max.AutoSize = true;
            this.check_max.Location = new System.Drawing.Point(15, 94);
            this.check_max.Name = "check_max";
            this.check_max.Size = new System.Drawing.Size(15, 14);
            this.check_max.TabIndex = 18;
            this.check_max.UseVisualStyleBackColor = true;
            this.check_max.CheckedChanged += new System.EventHandler(this.check_max_CheckedChanged);
            // 
            // num_min
            // 
            this.num_min.DecimalPlaces = 2;
            this.num_min.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_min.Location = new System.Drawing.Point(179, 66);
            this.num_min.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_min.Name = "num_min";
            this.num_min.Size = new System.Drawing.Size(75, 20);
            this.num_min.TabIndex = 19;
            this.num_min.Title = "הכנס ערך מינימאלי";
            // 
            // num_max
            // 
            this.num_max.DecimalPlaces = 2;
            this.num_max.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_max.Location = new System.Drawing.Point(179, 92);
            this.num_max.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.num_max.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_max.Name = "num_max";
            this.num_max.Size = new System.Drawing.Size(75, 20);
            this.num_max.TabIndex = 21;
            this.num_max.Title = "הכנס ערך מקסימאלי";
            this.num_max.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // num_multiply
            // 
            this.num_multiply.DecimalPlaces = 2;
            this.num_multiply.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.num_multiply.Location = new System.Drawing.Point(179, 124);
            this.num_multiply.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_multiply.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.num_multiply.Name = "num_multiply";
            this.num_multiply.Size = new System.Drawing.Size(75, 20);
            this.num_multiply.TabIndex = 22;
            this.num_multiply.Title = "הכנס הגבלה";
            this.num_multiply.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // num_rec
            // 
            this.num_rec.DecimalPlaces = 2;
            this.num_rec.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.num_rec.Location = new System.Drawing.Point(179, 153);
            this.num_rec.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_rec.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.num_rec.Name = "num_rec";
            this.num_rec.Size = new System.Drawing.Size(75, 20);
            this.num_rec.TabIndex = 23;
            this.num_rec.Title = "הכנס מספר חזרות";
            this.num_rec.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // check_multiply
            // 
            this.check_multiply.AutoSize = true;
            this.check_multiply.Location = new System.Drawing.Point(15, 126);
            this.check_multiply.Name = "check_multiply";
            this.check_multiply.Size = new System.Drawing.Size(15, 14);
            this.check_multiply.TabIndex = 24;
            this.check_multiply.UseVisualStyleBackColor = true;
            this.check_multiply.CheckedChanged += new System.EventHandler(this.check_multiply_CheckedChanged);
            // 
            // check_rec
            // 
            this.check_rec.AutoSize = true;
            this.check_rec.Location = new System.Drawing.Point(15, 155);
            this.check_rec.Name = "check_rec";
            this.check_rec.Size = new System.Drawing.Size(15, 14);
            this.check_rec.TabIndex = 25;
            this.check_rec.UseVisualStyleBackColor = true;
            this.check_rec.CheckedChanged += new System.EventHandler(this.check_rec_CheckedChanged);
            // 
            // SalesPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 319);
            this.Controls.Add(this.check_rec);
            this.Controls.Add(this.check_multiply);
            this.Controls.Add(this.num_rec);
            this.Controls.Add(this.num_multiply);
            this.Controls.Add(this.num_max);
            this.Controls.Add(this.num_min);
            this.Controls.Add(this.check_max);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.check_dates);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SalesPropertiesForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.Text = "SalesPropertiesForm";
            ((System.ComponentModel.ISupportInitialize)(this.num_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_multiply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_rec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.CheckBox check_dates;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.CheckBox check_max;
        private TouchableNumeric num_min;
        private TouchableNumeric num_max;
        private TouchableNumeric num_multiply;
        private TouchableNumeric num_rec;
        private System.Windows.Forms.CheckBox check_multiply;
        private System.Windows.Forms.CheckBox check_rec;
        private System.Windows.Forms.ToolTip HoverTexts;
    }
}