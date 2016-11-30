namespace SalesModule
{
    partial class SalesGUI1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesGUI));
            this.btnMngmnt = new System.Windows.Forms.Button();
            this.btn_lowPriced = new System.Windows.Forms.Button();
            this.btn_discountedProduct = new System.Windows.Forms.Button();
            this.btn_simpleBuyAndGet = new System.Windows.Forms.Button();
            this.btn_test = new System.Windows.Forms.Button();
            this.btn_advancedBundle = new System.Windows.Forms.Button();
            this.btn_advBuyAndGet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMngmnt
            // 
            resources.ApplyResources(this.btnMngmnt, "btnMngmnt");
            this.btnMngmnt.Name = "btnMngmnt";
            this.btnMngmnt.UseVisualStyleBackColor = true;
            this.btnMngmnt.Click += new System.EventHandler(this.btnMngmnt_Click);
            // 
            // btn_lowPriced
            // 
            resources.ApplyResources(this.btn_lowPriced, "btn_lowPriced");
            this.btn_lowPriced.Name = "btn_lowPriced";
            this.btn_lowPriced.UseVisualStyleBackColor = true;
            this.btn_lowPriced.Click += new System.EventHandler(this.btn_lowPriced_Click);
            // 
            // btn_discountedProduct
            // 
            resources.ApplyResources(this.btn_discountedProduct, "btn_discountedProduct");
            this.btn_discountedProduct.Name = "btn_discountedProduct";
            this.btn_discountedProduct.UseVisualStyleBackColor = true;
            this.btn_discountedProduct.Click += new System.EventHandler(this.btn_discountedProduct_Click);
            // 
            // btn_simpleBuyAndGet
            // 
            resources.ApplyResources(this.btn_simpleBuyAndGet, "btn_simpleBuyAndGet");
            this.btn_simpleBuyAndGet.Name = "btn_simpleBuyAndGet";
            this.btn_simpleBuyAndGet.UseVisualStyleBackColor = true;
            this.btn_simpleBuyAndGet.Click += new System.EventHandler(this.btn_simpleBuyAndGet_Click);
            // 
            // btn_test
            // 
            resources.ApplyResources(this.btn_test, "btn_test");
            this.btn_test.Name = "btn_test";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // btn_advancedBundle
            // 
            resources.ApplyResources(this.btn_advancedBundle, "btn_advancedBundle");
            this.btn_advancedBundle.Name = "btn_advancedBundle";
            this.btn_advancedBundle.UseVisualStyleBackColor = true;
            this.btn_advancedBundle.Click += new System.EventHandler(this.btn_advancedBundle_Click);
            // 
            // btn_advBuyAndGet
            // 
            resources.ApplyResources(this.btn_advBuyAndGet, "btn_advBuyAndGet");
            this.btn_advBuyAndGet.Name = "btn_advBuyAndGet";
            this.btn_advBuyAndGet.UseVisualStyleBackColor = true;
            this.btn_advBuyAndGet.Click += new System.EventHandler(this.btn_advBuyAndGet_Click);
            // 
            // SalesGUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_advBuyAndGet);
            this.Controls.Add(this.btn_advancedBundle);
            this.Controls.Add(this.btn_test);
            this.Controls.Add(this.btn_simpleBuyAndGet);
            this.Controls.Add(this.btn_discountedProduct);
            this.Controls.Add(this.btn_lowPriced);
            this.Controls.Add(this.btnMngmnt);
            this.Name = "SalesGUI";
            this.EnabledChanged += new System.EventHandler(this.SalesGUI_EnabledChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMngmnt;
        private System.Windows.Forms.Button btn_lowPriced;
        private System.Windows.Forms.Button btn_discountedProduct;
        private System.Windows.Forms.Button btn_simpleBuyAndGet;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.Button btn_advancedBundle;
        private System.Windows.Forms.Button btn_advBuyAndGet;


    }
}

