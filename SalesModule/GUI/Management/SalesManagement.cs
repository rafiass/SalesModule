using System.Data;
using System.Windows.Forms;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.GUI
{
    internal partial class SalesManagement : Form
    {
        public SalesManagement()
        {
            InitializeComponent();
            populate_DGV();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void populate_DGV()
        {
            dgv_sales.AutoGenerateColumns = false;
            dgv_sales.DataSource = DBService.GetService().GetAllSalesTitles();
        }

        private void dgv_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            var dt = senderGrid.DataSource as DataTable;
            if (e.RowIndex >= 0)
            {
                int groupID = int.Parse(dt.Rows[e.RowIndex]["GroupID"].ToString());
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    if (senderGrid.Columns[e.ColumnIndex].Name == "vip_cmd")
                    {
                        var f = new VIPAssociation();
                        f.ShowDialog(groupID);
                    }
                    else if (senderGrid.Columns[e.ColumnIndex].Name == "pcid_cmd")
                    {
                        var f = new PCIDAssociation();
                        f.ShowDialog(groupID);
                    }
                    else if (senderGrid.Columns[e.ColumnIndex].Name == "edit_cmd")
                    {
                        editSaleM(groupID);
                    }
                }
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                {
                    DBService.GetService().DisableSaleM(groupID,
                        !bool.Parse(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
                    populate_DGV();
                }
            }
        }

        private void editSaleM(int groupID)
        {
            SalesGroupM group = DBService.GetService().LoadGroup(groupID);
            if (group.Sales.Count == 1)
            {
                var sale = group.Sales[0];
                switch (sale.Type)
                {
                    case SaleTypes.SingularBuyAndGet: sale = SingularBuyAndGet.Edit(sale); break;
                    //case SaleTypes.Buy2GetAdvanced: sale = Buy2GetAdvancedForm.Edit(sale); break;
                    case SaleTypes.AdvancedBundle: sale = BundleAdvancedForm.Edit(sale); break;
                }
                if (sale != null)
                {
                    if (DBService.GetService().EditSaleM(sale))
                        MessageBox.Show("המבצע עודכן בהצלחה!");
                    else
                        MessageBox.Show("אירעה שגיאה בזמן עריכת המבצע.\nלא בוצעו שינויים במבצע הקיים.");

                }
            }
            else
            {
                //edit group form
            }
        }
    }
}
