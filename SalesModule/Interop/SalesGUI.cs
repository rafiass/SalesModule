using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using SalesModule.Models;
using SalesModule.Services;

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

        [DispId(6)]
        void Initialize();

        [DispId(7)]
        void Resize(int width, int heigth);
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
        public void Resize(int width, int heigth)
        {
            mainView1.Width = elementHost1.Width = Width = width;
            mainView1.Height = elementHost1.Height = Height = heigth;
        }
        private void SalesGUI_EnabledChanged(object sender, EventArgs e)
        {
            if (Enabled && Wrapper.User == null)
                Enabled = false;
        }
    }
}
