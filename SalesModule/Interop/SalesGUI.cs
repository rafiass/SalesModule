using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using SalesModule.GUI;

namespace SalesModule
{

    #region Interfaces

    /// <summary>
    /// This is the default interface implemented by the user control, and should
    /// contain all the methods and properties that will be exposed to COM.
    /// </summary>
    [Guid(SalesGUI.InterfaceId)]
    public interface ISalesGUI
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user control is visible.
        /// </summary>
        [DispId(1)]
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user control is enabled.
        /// </summary>
        [DispId(2)]
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the user control.
        /// </summary>
        [DispId(3)]
        int ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the user control.
        /// </summary>
        [DispId(4)]
        int BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background image of the user control.
        /// </summary>
        [DispId(5)]
        Image BackgroundImage { get; set; }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw 
        /// itself and any child controls.
        /// </summary>
        [DispId(6)]
        void Refresh();

        [DispId(7)]
        void Initialize();
    }
    #endregion

    /// <summary>
    /// Can be initialized only after Wrapper.Init is called.
    /// </summary>
    [Guid(ClassId), ClassInterface(ClassInterfaceType.None)]
    public partial class SalesGUI : UserControl, ISalesGUI
    {
        #region VB6 Interop Code

#if COM_INTEROP_ENABLED

        #region "COM Registration"

        // These  GUIDs provide the COM identity for this class 
        // and its COM interfaces. If you change them, existing 
        // clients will no longer be able to access the class.

        public const string ClassId = "3f415adf-bdff-49c1-8d79-5bf20859f34f";
        public const string InterfaceId = "367e9586-a89e-469e-9867-049bf12d1385";

        // These routines perform the additional COM registration needed by ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(System.Type t)
        {
            ComRegistration.RegisterControl(t);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(System.Type t)
        {
            ComRegistration.UnregisterControl(t);
        }


        #endregion

        #region "VB6 Properties"

        // The following are examples of how to expose typical form properties to VB6.  
        // You can also use these as examples on how to add additional properties.

        // Must declare this property as new as it exists in Windows.Forms and is not overridable
        public new bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        public new bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        public int ForegroundColor
        {
            get
            {
                return ActiveXControlHelpers.GetOleColorFromColor(base.ForeColor);
            }
            set
            {
                base.ForeColor = ActiveXControlHelpers.GetColorFromOleColor(value);
            }
        }

        public int BackgroundColor
        {
            get
            {
                return ActiveXControlHelpers.GetOleColorFromColor(base.BackColor);
            }
            set
            {
                base.BackColor = ActiveXControlHelpers.GetColorFromOleColor(value);
            }
        }

        public override Image BackgroundImage
        {
            get { return null; }
            set
            {
                if (null != value)
                {
                    MessageBox.Show("Setting the background image of an Interop UserControl is not supported, please use a PictureBox instead.", "Information");
                }
                base.BackgroundImage = null;
            }
        }

        #endregion

        #region "VB6 Methods"

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {

            const int WM_SETFOCUS = 0x7;
            const int WM_PARENTNOTIFY = 0x210;
            const int WM_DESTROY = 0x2;
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_RBUTTONDOWN = 0x204;

            if (m.Msg == WM_SETFOCUS)
            {
                // Raise Enter event
                this.OnEnter(System.EventArgs.Empty);
            }
            else if (m.Msg == WM_PARENTNOTIFY && (m.WParam.ToInt32() == WM_LBUTTONDOWN || m.WParam.ToInt32() == WM_RBUTTONDOWN))
            {

                if (!this.ContainsFocus)
                {
                    // Raise Enter event
                    this.OnEnter(System.EventArgs.Empty);
                }
            }
            else if (m.Msg == WM_DESTROY && !this.IsDisposed && !this.Disposing)
            {
                // Used to ensure that VB6 will cleanup control properly
                this.Dispose();
            }

            base.WndProc(ref m);
        }

        // This event will hook up the necessary handlers
        private void SalesGUI_ControlAdded(object sender, ControlEventArgs e)
        {
            ActiveXControlHelpers.WireUpHandlers(e.Control, ValidationHandler);
        }

        // Ensures that the Validating and Validated events fire appropriately
        internal void ValidationHandler(object sender, System.EventArgs e)
        {
            if (this.ContainsFocus) return;

            // Raise Leave event
            this.OnLeave(e);

            if (this.CausesValidation)
            {
                CancelEventArgs validationArgs = new CancelEventArgs();
                this.OnValidating(validationArgs);

                if (validationArgs.Cancel && this.ActiveControl != null)
                    this.ActiveControl.Focus();
                else
                {
                    // Raise Validated event
                    this.OnValidated(e);
                }
            }

        }

        #endregion

#endif

        #endregion

        /* ReadMe
         * Controls:
         * 1. ProductFinder - Search for product:
         *  - TextBox with auto-fill with product names
         *  - search by name or barcode with 'like' operator (where <field> like '%<Text>%')
         *  - Product typed member - if there is only one result product
         *
         * Sales types:
         *  Simple Sales
         *      * Common to all
         *          - a button for sales properties form
         *          - button for submit
         *      1. Discounted Product Form
         *          - ProductFinder
         *          - RadioButton options for discount type - fix price as default
         *          - CheckBox for adding a gift
         *              - if checked - enable another ProductFinder
         *          - CheckBox for minimum amount
         *              - if checked - enable a QTYAdjuster for choosing a positive natural number
         *      2. Discounted Category Form
         *          - search for category by name - search OnTextChanged
         *      3. buy and get - same product
         *          - ProductFinder
         *          - two QTYAdjusters - how many to buy? how many to get?
         *  Advanced Sales
         *      1. bundle
         *      2. Advanced buy and get
         *      3. 
         * 
         * 
         * 1. Bundle - (X+Y+Z..) for special price
         *  - set products table - pluno and qty
         *  - set fix price
         * 2. Buy & Get - buy X and get Y for a special price
         *  - select product and qty
         *  - select discounted product
         *  - select discount type
         *  - set discount value
         * 3. Receipt - buy over X$ and get Y for special price
         *  - set minimum receipt value
         *  - set products table to be discounted each with type and amount
         * 4. New Price - select product, set discount
        */

        public SalesGUI()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            this.ControlAdded += (SalesGUI_ControlAdded);
            Enabled = Wrapper.User != null;
        }
        public void Initialize()
        {
            Enabled = Wrapper.User != null;
            if (!Enabled)
                MessageBox.Show("אין אפשרות להפעיל מודל המבצעים.\nאירעה שגיאה בזמן טעינת המודל");
        }
        private void SalesGUI_EnabledChanged(object sender, EventArgs e)
        {
            if (Enabled && Wrapper.User == null)
                Enabled = false;
        }

        private void createNewSale(SaleTypes type)
        {
            Sale s = null;
            switch (type)
            {
                case SaleTypes.SingularLowerPrice: s = LowPriceProductForm.Create(); break;
                case SaleTypes.SingularBuyAndGet: s = SingularBuyAndGet.Create(); break;
                //case SaleTypes.Buy2GetAdvanced: s = Buy2GetAdvancedForm.Create(); break;
                case SaleTypes.AdvancedBundle: s = BundleAdvancedForm.Create(); break;
            }
            if (s != null)
            {
                if (DBService.GetService().InsertGroup(new SalesGroup(
                    Wrapper.User, DateTime.Now, true, s)) != -1)
                    MessageBox.Show("המבצע נוצר בהצלחה!");
                else
                    MessageBox.Show("אירעה שגיאה בזמן יצירת המבצע אנא פנה אל מרכז התמיכה");
            }
        }

        private void btnMngmnt_Click(object sender, EventArgs e)
        {
            var f = new SalesManagement();
            f.ShowDialog();
        }

        private void btn_lowPriced_Click(object sender, EventArgs e)
        {
            createNewSale(SaleTypes.SingularLowerPrice);
        }
        private void btn_discountedProduct_Click(object sender, EventArgs e)
        {
            //### btn_discountedProduct_Click do nothing...
        }
        private void btn_simpleBuyAndGet_Click(object sender, EventArgs e)
        {
            createNewSale(SaleTypes.SingularBuyAndGet);
        }
        private void btn_advBuyAndGet_Click(object sender, EventArgs e)
        {
            //### btn_advBuyAndGet_Click do nothing
        }
        private void btn_advancedBundle_Click(object sender, EventArgs e)
        {
            createNewSale(SaleTypes.AdvancedBundle);
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            (new TestForm()).ShowDialog();
            //DBService.GetService().ChangeDBDebug();
        }
    }
}
