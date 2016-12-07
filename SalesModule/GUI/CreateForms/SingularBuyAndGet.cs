using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesModule.Models;

namespace SalesModule.GUI
{
    internal partial class SingularBuyAndGet : Form
    {
        private bool _isEditing { get { return _index != -1; } }
        private int _index;
        private int _ID;
        private SalesPropertiesM _prop;
        private SaleM _assembled;
        private SingularBuyAndGet() : this(null) { }
        private SingularBuyAndGet(SaleM s)
        {
            InitializeComponent();
            _assembled = null;
            _index = s != null ? s.Index : -1;
            _ID = s != null ? s.SaleID : -1;
            _prop = s != null ? s.Properties :
                new SalesPropertiesM("קנה וקבל") { InstanceMultiply = 0 };
            LoadSaleM(s);
        }

        private void LoadSaleM()
        {
            find_buy.Text = "";
            num_buy.Value = 1;
            find_get.Text = "";
            num_get.Value = 1;
        }
        private void LoadSaleM(SaleM s)
        {
            if (s == null)
            {
                LoadSaleM();
                return;
            }
            if (s.ReqProducts == null || s.ReqProducts.Count != 1)
                throw new ArgumentException("Required's product data mismatch.");
            if (s.Discounted == null || s.Discounted.Count != 1)
                throw new ArgumentException("Discounted's product data mismatch.");

            find_buy.Find(s.ReqProducts[0]);
            num_buy.Value = (decimal)s.ReqProducts[0].Amount;
            find_get.Find(s.Discounted[0]);
            num_get.Value = (decimal)s.Discounted[0].MaxMultiply;
        }

        public static SaleM Create()
        {
            return Edit(null);
        }
        public static SaleM Edit(SaleM s)
        {
            try
            {
                var f = new SingularBuyAndGet(s);
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
                if (find_buy.SelectedProduct == null)
                    MessageBox.Show("אנא בחר מוצר לקנייה.");
                else if (find_get.SelectedProduct == null)
                    MessageBox.Show("אנא בחר מוצר לקבלה.");
                else isValid = true;
                if (!isValid) return;

                var reqs = new List<ProdAmountM>();
                reqs.Add(new ProdAmountM(find_buy.SelectedProduct.ID, true, (double)num_buy.Value));
                var outs = new List<DiscountedProductM>();
                outs.Add(new DiscountedProductM(find_get.SelectedProduct.ID, true,
                    0, (double)num_get.Value, new DiscountM(0, DiscountTypes.Fix_Price)));
                _assembled = new SaleM(SaleTypes.SingularBuyAndGet, _prop, reqs, outs, null, _isEditing ? _index : 1, _ID);
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
    }
}
