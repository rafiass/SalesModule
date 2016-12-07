using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SalesModule.Models;

namespace SalesModule.GUI
{
    internal partial class LowPriceProductForm : Form
    {
        private bool _isEditing { get { return _index != -1; } }
        private int _index;
        private int _ID;
        private SalesPropertiesM _prop;
        private SaleM _assembled;
        private LowPriceProductForm() : this(null) { }
        private LowPriceProductForm(SaleM s)
        {
            InitializeComponent();
            _assembled = null;
            _index = s != null ? s.Index : -1;
            _ID = s != null ? s.SaleID : -1;
            _prop = s != null ? s.Properties :
                new SalesPropertiesM("מוצר במבצע");
            LoadSaleM(s);
        }

        private void LoadSaleM()
        {
            find_buy.Text = "";
            num_amount.Enabled = check_amount.Checked = true;
            num_amount.Value = 1;
            num_max.Enabled = check_max.Checked = false;
            num_max.Value = 1;
            check_gift.Checked = false;
            find_gift.Text = "";
            discCntrl.Discount = new DiscountM(10, DiscountTypes.Fix_Discount);
        }
        private void LoadSaleM(SaleM s)
        {
            if (s == null)
            {
                LoadSaleM();
                return;
            }

            if (s.Discounted == null || s.Discounted.Count != 1 || s.Discounted[0].Discount == null)
                throw new InvalidOperationException("Discounted product data mismatch.");
            var discounted = s.Discounted[0];
            if (discounted.Discounted == null || discounted.Discounted.Count > 1)
                throw new InvalidOperationException("Gifted product data mismatch.");
            find_buy.Find(discounted);

            num_amount.Enabled = check_amount.Checked = discounted.Amount != 0;
            num_amount.Value = (decimal)(discounted.Amount != 0 ? discounted.Amount : 1);
            num_max.Enabled = check_max.Checked = discounted.MaxMultiply != 0;
            num_max.Value = (decimal)(discounted.MaxMultiply != 0 ? discounted.MaxMultiply : 1);
            check_gift.Checked = discounted.Discounted.Count > 0;
            if (discounted.Discounted.Count > 0)
                find_gift.Find(discounted.Discounted[0]);
            discCntrl.Discount = discounted.Discount;
        }

        public static SaleM Create()
        {
            return Edit(null);
        }
        public static SaleM Edit(SaleM s)
        {
            try
            {
                var f = new LowPriceProductForm(s);
                return f.ShowDialog() == DialogResult.OK ?
                    f._assembled : null;
            }
            catch
            {
                return null;
            }
        }

        private void btn_prop_Click(object sender, EventArgs e)
        {
            var prop = new SalesPropertiesForm(_prop);
            prop.RecurrenceEnabled = false;
            prop.DatesEnabled = !_isEditing;
            if (prop.ShowDialog() == DialogResult.OK)
                _prop = prop.Properties;
        }
        private void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = false;
                var disc = discCntrl.Discount;
                if (find_buy.SelectedProduct == null)
                    MessageBox.Show("אנא בחר מוצר לקנייה.");
                else if (disc == null)
                    MessageBox.Show("נתוני ההנחה אינם תקינים.");
                else if (check_gift.Checked && find_gift.SelectedProduct == null)
                    MessageBox.Show("אנא בחר מוצר במתנה.");
                else isValid = true;
                if (!isValid) return;

                var outs = new List<DiscountedProductM>();
                outs.Add(new DiscountedProductM(find_buy.SelectedProduct.ID, true,
                    check_amount.Checked ? (double)num_amount.Value : 0,
                    check_max.Checked ? (double)num_max.Value : 0, disc,
                    !check_gift.Checked ? null : new GiftedProductM(find_gift.SelectedProduct,
                        1, new DiscountM(0, DiscountTypes.Fix_Price))));
                _prop.RecurrencePerInstance = 1;
                _assembled = new SaleM(SaleTypes.SingularLowerPrice, _prop,
                    null, outs, null, _isEditing ? _index : 1, _ID);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("אירעה שגיאה, נסה שוב מאוחר יותר.");
            }
        }
        private void btn_cncl_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void check_max_CheckedChanged(object sender, EventArgs e)
        {
            num_max.Enabled = check_max.Checked;
        }
        private void check_amount_CheckedChanged(object sender, EventArgs e)
        {
            num_amount.Enabled = check_amount.Checked;
        }
        private void check_gift_CheckedChanged(object sender, EventArgs e)
        {
            find_gift.Enabled = check_gift.Checked;
        }
    }
}
