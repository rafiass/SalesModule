using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using SalesModule.Models;

namespace SalesModule.GUI
{
    internal partial class BundleAdvancedForm : Form
    {
        private bool _isEditing { get { return _index != -1; } }
        private int _index;
        private int _ID;
        private SalesProperties _prop;
        private SaleM _assembled;
        private List<Tuple<ProdAmountM, string>> _bundle;
        private BundleAdvancedForm() : this(null) { }
        private BundleAdvancedForm(SaleM s)
        {
            InitializeComponent();
            _assembled = null;
            _index = s != null ? s.Index : -1;
            _ID = s != null ? s.SaleID : -1;
            _prop = s != null ? s.Properties :
                new SalesProperties("חבילת מוצרים במבצע");
            if (s != null && (s.ReqProducts == null || s.ReqProducts.Count == 0))
                throw new InvalidOperationException("Bundle data mismatch.");
            LoadSaleM(s);
        }

        private void LoadSaleM(SaleM s)
        {
            _bundle = new List<Tuple<ProdAmountM, string>>();
            if (s == null)
            {
                num_price.Value = (decimal)100;
                return;
            }

            if (s.Discount == null || s.Discount.Type != DiscountTypes.Fix_Price)
                throw new InvalidOperationException("Bundle's discount mismatch");

            s.ReqProducts.ForEach(req =>
            {
                var news = new ProductFinder();
                news.Find(s.ReqProducts[0]);
                _bundle.Add(new Tuple<ProdAmountM, string>(req, news.SelectedProduct.Name));
            });
            populate_DGV();
            num_price.Value = (decimal)s.Discount.Amount;
        }

        public static SaleM Create()
        {
            return Edit(null);
        }
        public static SaleM Edit(SaleM s)
        {
            try
            {
                var f = new BundleAdvancedForm(s);
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
            prop.MaxPriceEnabled = false;
            prop.DatesEnabled = !_isEditing;
            if (prop.ShowDialog() == DialogResult.OK)
                _prop = prop.Properties;
        }
        private void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_bundle.Count == 0)
                {
                    MessageBox.Show("אנא הכנס מוצרים לחבילה.");
                    return;
                }

                var reqs = _bundle.ConvertAll<ProdAmountM>(p => p.Item1);
                _assembled = new SaleM(SaleTypes.AdvancedBundle, _prop, reqs, null,
                    new Discount((double)num_price.Value, DiscountTypes.Fix_Price), _isEditing ? _index : 1, _ID);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("אירעה שגיאה, נסה שוב מאוחר יותר.");
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var ps = new ProductSelector();
            ps.ShowDialog();
            if (ps.SelectedProduct != null)
            {
                var prod = new ProdAmountM(ps.SelectedProduct.ID, ps.SelectedProduct.isPluno, 1);
                _bundle.Add(new Tuple<ProdAmountM, string>(prod, ps.SelectedProduct.Name));
                populate_DGV();
            }
        }
        private void populate_DGV()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
                new DataColumn("pname"), new DataColumn("qty"),new DataColumn("type")});

            _bundle.ForEach(p =>
            {
                var R = dt.NewRow();
                R["pname"] = p.Item2;
                R["qty"] = p.Item1.Amount;
                R["type"] = p.Item1.isPluno ? "מוצר" : "קטגוריה";
                dt.Rows.Add(R);
            });

            dgv_bundle.AutoGenerateColumns = false;
            dgv_bundle.DataSource = dt;
        }
        private void dgv_bundle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            if (e.RowIndex >= 0)
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    _bundle.RemoveAt(e.RowIndex);
                    populate_DGV();
                }
        }
        private void dgv_bundle_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            if (e.RowIndex >= 0)
                if (senderGrid.Columns[e.ColumnIndex].Name == "bundle_amount")
                {
                    double newPrice;
                    if (!double.TryParse(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out newPrice))
                        MessageBox.Show("ערך לא חוקי!");
                    else
                        _bundle[e.RowIndex].Item1.Amount = newPrice;
                    populate_DGV();
                }
        }
        private void dgv_bundle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            if (e.RowIndex >= 0)
                if (senderGrid.Columns[e.ColumnIndex].Name == "bundle_amount")
                {
                    e.Value = _bundle[e.RowIndex].Item1.Amount;
                    e.FormattingApplied = true;
                }
            e.FormattingApplied = false;
        }
    }
}
