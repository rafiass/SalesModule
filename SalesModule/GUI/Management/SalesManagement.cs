using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                        editSale(groupID);
                    }
                }
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                {
                    DBService.GetService().DisableSale(groupID,
                        !bool.Parse(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
                    populate_DGV();
                }
            }
        }

        private void editSale(int groupID)
        {
            SalesGroup group = DBService.GetService().LoadGroup(groupID);
            if (group.Sales.Count == 1)
            {
                var sale = group.Sales[0];
                switch (sale.Type)
                {
                    case SaleTypes.SingularLowerPrice: sale = LowPriceProductForm.Edit(sale); break;
                    case SaleTypes.SingularBuyAndGet: sale = SingularBuyAndGet.Edit(sale); break;
                    //case SaleTypes.Buy2GetAdvanced: sale = Buy2GetAdvancedForm.Edit(sale); break;
                    case SaleTypes.AdvancedBundle: sale = BundleAdvancedForm.Edit(sale); break;
                }
                if (sale != null)
                {
                    if (DBService.GetService().EditSale(sale))
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
