namespace SalesModule.GUI
{
    partial class BuyAndGetAdvancedForm
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
            this.DGV_Discounted = new System.Windows.Forms.DataGridView();
            this.btn_ok = new System.Windows.Forms.Button();
            this.txt_title = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_add_reqs = new System.Windows.Forms.Button();
            this.DGV_reqs = new System.Windows.Forms.DataGridView();
            this.btn_add_disc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Discounted)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_reqs)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_Discounted
            // 
            this.DGV_Discounted.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Discounted.Location = new System.Drawing.Point(15, 243);
            this.DGV_Discounted.Name = "DGV_Discounted";
            this.DGV_Discounted.Size = new System.Drawing.Size(417, 108);
            this.DGV_Discounted.TabIndex = 0;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(354, 357);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "בצע";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // txt_title
            // 
            this.txt_title.Location = new System.Drawing.Point(82, 43);
            this.txt_title.Name = "txt_title";
            this.txt_title.Size = new System.Drawing.Size(350, 20);
            this.txt_title.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(12, 46);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "שם המבצע:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(143, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 24);
            this.label6.TabIndex = 41;
            this.label6.Text = "מבצע \'קנה וקבל\'";
            // 
            // btn_add_reqs
            // 
            this.btn_add_reqs.Location = new System.Drawing.Point(322, 71);
            this.btn_add_reqs.Name = "btn_add_reqs";
            this.btn_add_reqs.Size = new System.Drawing.Size(110, 23);
            this.btn_add_reqs.TabIndex = 44;
            this.btn_add_reqs.Text = "הוסף מוצר נדרש";
            this.btn_add_reqs.UseVisualStyleBackColor = true;
            this.btn_add_reqs.Click += new System.EventHandler(this.btn_add_reqs_Click);
            // 
            // DGV_reqs
            // 
            this.DGV_reqs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_reqs.Location = new System.Drawing.Point(12, 100);
            this.DGV_reqs.Name = "DGV_reqs";
            this.DGV_reqs.Size = new System.Drawing.Size(417, 108);
            this.DGV_reqs.TabIndex = 46;
            // 
            // btn_add_disc
            // 
            this.btn_add_disc.Location = new System.Drawing.Point(319, 214);
            this.btn_add_disc.Name = "btn_add_disc";
            this.btn_add_disc.Size = new System.Drawing.Size(110, 23);
            this.btn_add_disc.TabIndex = 47;
            this.btn_add_disc.Text = "הוסף מוצר נדרש";
            this.btn_add_disc.UseVisualStyleBackColor = true;
            this.btn_add_disc.Click += new System.EventHandler(this.btn_add_disc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 219);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "מוצרים שינתנו בהנחה:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "מוצרים נדרשים למבצע:";
            // 
            // BuyAndGetAdvancedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 388);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_add_disc);
            this.Controls.Add(this.DGV_reqs);
            this.Controls.Add(this.btn_add_reqs);
            this.Controls.Add(this.txt_title);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.DGV_Discounted);
            this.Name = "BuyAndGetAdvancedForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.Text = "BuyAndGetAdvancedForm";
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Discounted)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_reqs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Discounted;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TextBox txt_title;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_add_reqs;
        private System.Windows.Forms.DataGridView DGV_reqs;
        private System.Windows.Forms.Button btn_add_disc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}