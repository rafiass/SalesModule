using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SalesModule.GUI
{
    internal class ProductFinder : TextBox, IMessageFilter
    {
        private Control ComboParentForm; // Or use type "Form" 
        private ListBox listBoxChild;
        private bool MsgFilterActive = false;
        private List<IProduct> _products;
        private IProduct _product;

        private List<IProduct> Products
        {
            get
            {
                if (_products == null)
                    _products = DBService.GetService().GetProducts();
                return _products;
            }
        }

        public IProduct SelectedProduct
        {
            get { return _product; }
            set
            {
                if (_product != value)
                {
                    _product = value;
                    ControlPaint.DrawBorder(CreateGraphics(), ClientRectangle,
                        value == null ? Color.Red : Color.LightGreen, ButtonBorderStyle.Solid);
                    if (OnProductChanged != null)
                        OnProductChanged(_product);
                }
            }
        }
        public event Action<IProduct> OnProductChanged;

        public ProductFinder()
            : base()
        {
            // Set up all the events we need to handle
            TextChanged += ComboListMatcher_TextChanged;
            LostFocus += ComboListMatcher_LostFocus;
            MouseDown += ComboListMatcher_MouseDown;
            HandleDestroyed += ComboListMatcher_HandleDestroyed;
            SelectedProduct = null;
            _products = null;
        }
        ~ProductFinder()
        {
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x201) // Mouse click: WM_LBUTTONDOWN
            {
                var Pos = new Point((int)(m.LParam.ToInt32() & 0xFFFF), (int)(m.LParam.ToInt32() >> 16));

                var Ctrl = Control.FromHandle(m.HWnd);
                if (Ctrl != null)
                {
                    // Convert the point into our parent control's coordinates ...
                    Pos = ComboParentForm.PointToClient(Ctrl.PointToScreen(Pos));

                    // ... because we need to hide the list if user clicks on something other than the list-box
                    if (ComboParentForm != null)
                    {
                        if (listBoxChild != null &&
                            (Pos.X < listBoxChild.Left || Pos.X > listBoxChild.Right || Pos.Y < listBoxChild.Top || Pos.Y > listBoxChild.Bottom))
                        {
                            this.HideTheList();
                        }
                    }
                }
            }
            else if (m.Msg == 0x100) // WM_KEYDOWN
            {
                if (listBoxChild != null && listBoxChild.Visible)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case 0x1B: // Escape key
                            this.HideTheList();
                            return true;

                        case 0x26: // up key
                        case 0x28: // right key
                            // Change selection
                            int NewIx = listBoxChild.SelectedIndex + ((m.WParam.ToInt32() == 0x26) ? -1 : 1);

                            // Keep the index valid!
                            if (NewIx >= 0 && NewIx < listBoxChild.Items.Count)
                                listBoxChild.SelectedIndex = NewIx;
                            return true;

                        case 0x0D: // return (use the currently selected item)
                            CopySelection();
                            return true;
                    }
                }
            }

            return false;
        }
        public void Find(ComperableProduct prod)
        {
            Find(prod.ID, prod.isPluno);
        }
        public void Find(string ID, bool ispluno)
        {
            var list = Products.FindAll(p => p.ID == ID && p.isPluno == ispluno);
            if (list.Count == 1)
                Text = list[0].Name;
            HideTheList();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            ProductSelector win = new ProductSelector();
            if (win.ShowDialog() == DialogResult.OK)
                Find(win.SelectedProduct.ID, win.SelectedProduct.isPluno);
        }

        private void ComboListMatcher_HandleDestroyed(object sender, EventArgs e)
        {
            if (MsgFilterActive)
                Application.RemoveMessageFilter(this);
        }
        private void ComboListMatcher_MouseDown(object sender, MouseEventArgs e)
        {
            HideTheList();
        }
        private void ComboListMatcher_LostFocus(object sender, EventArgs e)
        {
            if (listBoxChild != null && !listBoxChild.Focused)
                HideTheList();
        }
        private void InitListControl()
        {
            if (listBoxChild == null)
            {
                // Find parent - or keep going up until you find the parent form
                ComboParentForm = this.Parent;

                if (ComboParentForm != null)
                {
                    // Setup a messaage filter so we can listen to the keyboard
                    if (!MsgFilterActive)
                    {
                        Application.AddMessageFilter(this);
                        MsgFilterActive = true;
                    }

                    listBoxChild = new ListBox();
                    listBoxChild.Visible = false;
                    listBoxChild.Click += listBox1_Click;
                    ComboParentForm.Controls.Add(listBoxChild);
                    ComboParentForm.Controls.SetChildIndex(listBoxChild, 0); // Put it at the front
                }
            }
        }
        private bool isContain(string searchIn, string searchFor)
        {
            if (searchFor.Length > searchIn.Length) return false;
            return searchIn.ToLower().Substring(0, searchFor.Length) == searchFor.ToLower();
            //            (Item.Name.ToLower().Substring(0, SearchText.Length) == SearchText.ToLower() ||
            //            Item.Barcode.ToLower().Substring(0, SearchText.Length) == SearchText.ToLower()))
        }
        private void ComboListMatcher_TextChanged(object sender, EventArgs e)
        {
            InitListControl();

            if (listBoxChild == null)
                return;

            string SearchText = this.Text;

            listBoxChild.Items.Clear();

            // Don't show the list when nothing has been typed
            if (!string.IsNullOrEmpty(SearchText))
            {
                var list = new List<IProduct>();
                foreach (var Item in this.Products)
                    if (Item != null &&
                        (isContain(Item.Name, SearchText) ||
                        (Item is Product && isContain((Item as Product).Barcode, SearchText))))
                        list.Add(Item);
                listBoxChild.Items.AddRange(list.ConvertAll<string>(p => p.Name).ToArray());
            }
            SelectedProduct = Products.Find(p => p.Name == SearchText ||
                (p is Product && (p as Product).Barcode == SearchText));

            if (listBoxChild.Items.Count > 0)
            {
                Point PutItHere = new Point(this.Left, this.Bottom);
                Control TheControlToMove = this;

                PutItHere = this.Parent.PointToScreen(PutItHere);

                TheControlToMove = listBoxChild;
                PutItHere = ComboParentForm.PointToClient(PutItHere);

                TheControlToMove.Show();
                TheControlToMove.Left = PutItHere.X;
                TheControlToMove.Top = PutItHere.Y;
                TheControlToMove.Width = this.Width;

                int TotalItemHeight = listBoxChild.ItemHeight * (listBoxChild.Items.Count + 1);
                TheControlToMove.Height = Math.Min(ComboParentForm.ClientSize.Height - TheControlToMove.Top, TotalItemHeight);
            }
            else
                HideTheList();
        }
        /// <summary>
        /// Copy the selection from the list-box into the combo box
        /// </summary>
        private void CopySelection()
        {
            if (listBoxChild.SelectedItem != null)
            {
                SelectedProduct = Products.Find(p => p.Name == listBoxChild.SelectedItem.ToString());
                Text = listBoxChild.SelectedItem.ToString();
                HideTheList();
                this.SelectAll();
            }
        }
        private void listBox1_Click(object sender, EventArgs e)
        {
            var ThisList = sender as ListBox;

            if (ThisList != null)
            {
                // Copy selection to the combo box
                CopySelection();
            }
        }
        private void HideTheList()
        {
            if (listBoxChild != null)
                listBoxChild.Hide();
        }
    }
}
