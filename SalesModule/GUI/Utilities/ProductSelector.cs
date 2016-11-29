using System;
using System.Data;
using System.Windows.Forms;

namespace SalesModule
{
    internal partial class ProductSelector : Form
    {
        private DBService _service;
        public IProduct SelectedProduct { get; private set; }
        public bool IsValid { get; private set; }

        public ProductSelector()
        {
            InitializeComponent();
            _service = DBService.GetService();
            IsValid = false;
            SelectedProduct = null;
        }

        private void ProductSelector_Load(object sender, EventArgs e)
        {
            txtVal.Text = "";
            populateTypes();
            DGVProducts.DataSource = null;
        }

        private void populateTypes()
        {
            DDLTypes.Items.Clear();
            DDLTypes.Items.Add("מק\"ט");
            DDLTypes.Items.Add("שם");
            DDLTypes.Items.Add("ברקוד");
            DDLTypes.SelectedIndex = 0;
        }

        private void populateDGV()
        {
            string val = txtVal.Text;
            DataTable dt;
            if (val == "")
            {
                DGVProducts.DataSource = null;
                return;
            }
            switch (DDLTypes.SelectedIndex)
            {
                case 0: dt = _service.SearchProductsByPluno(val); break;
                case 1: dt = _service.SearchProductsByName(val); break;
                case 2: dt = _service.SearchProductsByBarcode(val); break;
                default: dt = null; break;
            }
            DGVProducts.AutoGenerateColumns = false;
            DGVProducts.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            populateDGV();
        }

        private void DGVProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
                DGVProducts.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                var senderGrid = sender as DataGridView;
                var R = DGVProducts.Rows[e.RowIndex];
                string kind = (senderGrid.DataSource as DataTable).Rows[e.RowIndex]["kind3"].ToString();
                SelectedProduct = new Product(
                    R.Cells["pluno"].Value.ToString(),
                    R.Cells["pname"].Value.ToString(),
                    R.Cells["barcode"].Value.ToString(),
                    kind == "" ? (int?)null : int.Parse(kind));
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
