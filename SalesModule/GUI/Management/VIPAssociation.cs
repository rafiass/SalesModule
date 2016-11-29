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
    internal partial class VIPAssociation : Form
    {
        private int _groupID;
        public VIPAssociation()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Enabled = false;
            _groupID = -1;
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (Enabled && _groupID == -1) Enabled = false;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        public void ShowDialog(int groupID)
        {
            _groupID = groupID;
            Enabled = true;
            Text = "ניהול לקוחות עבור מבצע (#" + groupID + ")";
            pop_assoc();
            pop_groups();
            pop_singles();

            ShowDialog();
        }

        private void pop_assoc()
        {
            DGVassoc.AutoGenerateColumns = false;
            DGVassoc.DataSource = DBService.GetService().GetSalesVIPs(_groupID);
        }
        private void pop_singles()
        {
            DGVsingle.AutoGenerateColumns = false;
            DGVsingle.DataSource = DBService.GetService().GetVIPSingles(_groupID);
        }
        private void pop_groups()
        {
            DGVgroup.AutoGenerateColumns = false;
            DGVgroup.DataSource = DBService.GetService().GetVIPGroups(_groupID);
        }

        private void DGVsingle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var dt = senderGrid.DataSource as DataTable;
                DBService.GetService().AssociateVIP2Sale(_groupID,
                    int.Parse(dt.Rows[e.RowIndex]["vipno"].ToString()), true);
                pop_assoc();
                pop_singles();
            }
        }
        private void DGVgroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var dt = senderGrid.DataSource as DataTable;
                DBService.GetService().AssociateVIP2Sale(_groupID,
                    int.Parse(dt.Rows[e.RowIndex]["groupID"].ToString()), false);
                pop_assoc();
                pop_groups();
            }
        }
        private void DGVassoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var dt = senderGrid.DataSource as DataTable;
                DBService.GetService().DisassociateVIPfromSale(_groupID,
                    int.Parse(dt.Rows[e.RowIndex]["VipID"].ToString()),
                    bool.Parse(dt.Rows[e.RowIndex]["isVipno"].ToString()));
                pop_assoc();
                pop_singles();
                pop_groups();
            }
        }

        private void DGVassoc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var dt = (sender as DataGridView).DataSource as DataTable;
                if (e.ColumnIndex == 1)
                {
                    e.Value = bool.Parse(dt.Rows[e.RowIndex]["isVipno"].ToString()) ?
                        "פרטי" : "מועדון";
                    e.FormattingApplied = true;
                    return;
                }
            }
            catch
            {

            }
            e.FormattingApplied = false;
        }
    }
}
