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
    internal partial class SalesPropertiesForm : Form
    {
        public bool MaxPriceEnabled
        {
            get { return check_max.Enabled; }
            set
            {
                num_max.Enabled = check_max.Enabled = value;
                if (!value)
                    check_max.Checked = false;
            }
        }
        public bool InstancePerMinEnabled
        {
            get { return check_multiply.Enabled; }
            set
            {
                num_multiply.Enabled = check_multiply.Enabled = value;
                if (!value)
                    check_multiply.Checked = false;
            }
        }
        public bool RecurrenceEnabled
        {
            get { return check_rec.Enabled; }
            set
            {
                num_rec.Enabled = check_rec.Enabled = value;
                if (!value)
                    check_rec.Checked = false;
            }
        }
        public bool DatesEnabled
        {
            get { return check_dates.Enabled; }
            set
            {
                check_dates.Enabled = value;
                dateTimePicker1.Enabled = dateTimePicker2.Enabled = value;
            }
        }

        public SalesProperties Properties { get; private set; }

        public SalesPropertiesForm(SalesProperties prop)
        {
            InitializeComponent();
            Properties = prop;
            populate();
        }

        private void populate()
        {
            txt_name.Text = Properties.Title;

            num_max.Enabled = check_max.Checked = Properties.MaxPrice != null;
            num_multiply.Enabled = check_multiply.Checked = Properties.InstanceMultiply != 0;
            num_rec.Enabled = check_rec.Checked = Properties.RecurrencePerInstance != 0;

            num_min.Value = (decimal)Properties.MinPrice;
            if (check_max.Checked)
                num_max.Value = (decimal)Properties.MaxPrice;
            if (check_multiply.Checked)
                num_multiply.Value = (decimal)Properties.InstanceMultiply;
            if (check_rec.Checked)
                num_rec.Value = (decimal)Properties.RecurrencePerInstance;

            check_dates.Checked = Properties.IsBroadSale;
            dateTimePicker1.Value = Properties.DateFrom;
            if (Properties.DateTo != null)
                dateTimePicker2.Value = Properties.DateTo ?? DateTime.Now + new TimeSpan(7, 0, 0, 0);
            else
                dateTimePicker2.Checked = false;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (txt_name.Text.Trim() == "")
                MessageBox.Show("שם מבצע לא תקין.");
            else if (check_max.Checked && num_max.Value <= num_min.Value)
                MessageBox.Show("ערכי מינימום ומקסימום של שווי הסל לא מתאימים.");
            else if (check_dates.Checked && dateTimePicker2.Checked &&
                (dateTimePicker2.Value < dateTimePicker1.Value ||
                dateTimePicker2.Value < DateTime.Now))
                MessageBox.Show("טווח התאריכים אינו תקין");
            else
            {
                Properties = new SalesProperties(txt_name.Text, (double)num_min.Value,
                    check_max.Checked ? (double?)num_max.Value : null,
                    check_multiply.Checked ? (int)num_multiply.Value : 0,
                    check_rec.Checked ? (int)num_rec.Value : 0,
                    check_dates.Checked, dateTimePicker1.Value,
                    dateTimePicker2.Checked ? dateTimePicker2.Value : (DateTime?)null);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void txt_name_Leave(object sender, EventArgs e)
        {
            if (txt_name.Text == "")
                txt_name.Text = Properties.Title;
            else
                Properties.Title = txt_name.Text;
        }

        private void check_dates_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = dateTimePicker2.Enabled = check_dates.Checked;
        }
        private void check_max_CheckedChanged(object sender, EventArgs e)
        {
            num_max.Enabled = check_max.Checked;
        }
        private void check_multiply_CheckedChanged(object sender, EventArgs e)
        {
            num_multiply.Enabled = check_multiply.Checked;
        }
        private void check_rec_CheckedChanged(object sender, EventArgs e)
        {
            num_rec.Enabled = check_rec.Checked;
        }
    }
}
