namespace SalesTester
{
    partial class MainWindow
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
            this.btn_create = new System.Windows.Forms.Button();
            this.btn_manual = new System.Windows.Forms.Button();
            this.btn_auto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_create
            // 
            this.btn_create.Location = new System.Drawing.Point(12, 12);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(269, 42);
            this.btn_create.TabIndex = 0;
            this.btn_create.Text = "יצירת מבצע";
            this.btn_create.UseVisualStyleBackColor = true;
            this.btn_create.Click += new System.EventHandler(this.btn_create_Click);
            // 
            // btn_manual
            // 
            this.btn_manual.Location = new System.Drawing.Point(12, 60);
            this.btn_manual.Name = "btn_manual";
            this.btn_manual.Size = new System.Drawing.Size(131, 55);
            this.btn_manual.TabIndex = 1;
            this.btn_manual.Text = "בדיקה ידנית";
            this.btn_manual.UseVisualStyleBackColor = true;
            this.btn_manual.Click += new System.EventHandler(this.btn_manual_Click);
            // 
            // btn_auto
            // 
            this.btn_auto.Location = new System.Drawing.Point(150, 60);
            this.btn_auto.Name = "btn_auto";
            this.btn_auto.Size = new System.Drawing.Size(131, 55);
            this.btn_auto.TabIndex = 2;
            this.btn_auto.Text = "בדיקות אוטומתיות";
            this.btn_auto.UseVisualStyleBackColor = true;
            this.btn_auto.Click += new System.EventHandler(this.btn_auto_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 125);
            this.Controls.Add(this.btn_auto);
            this.Controls.Add(this.btn_manual);
            this.Controls.Add(this.btn_create);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_create;
        private System.Windows.Forms.Button btn_manual;
        private System.Windows.Forms.Button btn_auto;

    }
}

