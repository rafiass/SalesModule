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
    internal partial class PCIDAssociation : Form
    {
        private int _saleID;
        public PCIDAssociation()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Enabled = false;
            panel_reg.Enabled = false;
            _saleID = -1;
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (Enabled && _saleID == -1) Enabled = false;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        public void ShowDialog(int saleID)
        {
            _saleID = saleID;
            Enabled = true;
            Text = "ניהול סניפים עבור מבצע (#" + saleID + ")";
            populate_DGV();
            populate_panel();

            ShowDialog();
        }

        private void populate_panel()
        {
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = (DateTime.Now + (new TimeSpan(7, 0, 0, 0))).Date;

            pop_combo_hours(combo_from);
            combo_from.SelectedIndex = 0;
            pop_combo_hours(combo_to);
            combo_to.SelectedIndex = 48;

            pop_combo_pcid();
        }
        private void populate_DGV()
        {
            DGV_assoc.AutoGenerateColumns = false;
            DGV_assoc.DataSource = DBService.GetService().GetSalesBranches(_saleID);
        }

        private void pop_combo_pcid()
        {
            combo_pcid.DisplayMember = "bhname";
            combo_pcid.ValueMember = "bhno";
            var dt = DBService.GetService().GetUnattachedPcid(_saleID);
            var r = dt.NewRow();
            r["bhname"] = "בחר סניף";
            r["bhno"] = -1;
            dt.Rows.InsertAt(r, 0);
            combo_pcid.DataSource = dt;
            panel_reg.Enabled = combo_pcid.Items.Count > 1;
        }

        private static void pop_combo_hours(ComboBox cb)
        {
            cb.Items.Clear();
            cb.DisplayMember = "Value";
            cb.ValueMember = "Key";
            for (int i = 0; i < 48; i++)
                cb.Items.Add(new KeyValuePair<int, string>(i,
                    (i / 2 < 10 ? "0" : "") + (i / 2) + (i % 2 == 0 ? ":00" : ":30")));

            cb.Items.Add(new KeyValuePair<int, string>(48, "23:59"));
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            bool valid = false;
            if (combo_pcid.SelectedIndex == 0)
                MessageBox.Show("אנא בחר סניף");
            else if (dateTimePicker2.Checked &&
                (dateTimePicker2.Value < dateTimePicker1.Value ||
                dateTimePicker2.Value < DateTime.Now))
                MessageBox.Show("טווח התאריכים אינו תקין");
            else if (combo_to.SelectedIndex <= combo_from.SelectedIndex)
                MessageBox.Show("טווח השעות אינו תקין");
            else
                valid = true;

            if (!valid) return;

            TimeSpan tFrom = new TimeSpan(0, 30 * combo_from.SelectedIndex, 0);
            TimeSpan tTo = new TimeSpan(0, 30 * combo_to.SelectedIndex, 0);
            if (combo_to.SelectedIndex == 48) tTo -= new TimeSpan(0, 0, 1);
            DateTime? to = dateTimePicker2.Checked ? dateTimePicker2.Value : (DateTime?)null;

            DBService.GetService().AssociatePcid2Sale(_saleID,
                int.Parse(combo_pcid.SelectedValue.ToString()),
                dateTimePicker1.Value, to, tFrom, tTo);

            populate_DGV();
            populate_panel();
        }

        private void DGV_assoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            var dt = senderGrid.DataSource as DataTable;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DBService.GetService().DisassociatePcidfromSale(_saleID,
                    int.Parse(dt.Rows[e.RowIndex]["pcid"].ToString()));
                populate_DGV();
                pop_combo_pcid();
            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                e.RowIndex >= 0)
            {
                DBService.GetService().DisablePCID(_saleID,
                    int.Parse(dt.Rows[e.RowIndex]["pcid"].ToString()),
                    !bool.Parse(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
                populate_DGV();
            }
        }
    }
}
