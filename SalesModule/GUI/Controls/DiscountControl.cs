using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesModule.Models;

namespace SalesModule.GUI
{
    internal partial class DiscountControl : UserControl
    {
        public DiscountM Discount
        {
            get
            {
                if (rad_fix_price.Checked)
                    return new DiscountM((double)num_fix.Value, DiscountTypes.Fix_Price);
                else if (rad_fix_disc.Checked)
                    return new DiscountM((double)num_disc.Value, DiscountTypes.Fix_Discount);
                else if (rad_percentage.Checked)
                    return new DiscountM((double)num_percentage.Value, DiscountTypes.Percentage);
                else return null;
            }
            set
            {
                if (value == null) return;
                init();
                switch (value.Type)
                {
                    case DiscountTypes.Fix_Price:
                        rad_fix_price.Checked = true;
                        num_fix.Value = (decimal)value.Amount;
                        break;
                    case DiscountTypes.Fix_Discount:
                        rad_fix_disc.Checked = true;
                        num_disc.Value = (decimal)value.Amount;
                        break;
                    case DiscountTypes.Percentage:
                        rad_percentage.Checked = true;
                        num_percentage.Value = (decimal)value.Amount;
                        break;
                }
            }
        }

        public DiscountControl()
        {
            InitializeComponent();
            num_disc.ValueChanged += (s, e) => setLabel();
            num_fix.ValueChanged += (s, e) => setLabel();
            num_percentage.ValueChanged += (s, e) => setLabel();
            init();
        }
        private void init()
        {
            rad_fix_price.Checked = rad_fix_disc.Checked = rad_percentage.Checked = true;
            rad_fix_disc.Checked = true;
            num_disc.Value = num_fix.Value = num_percentage.Value = 10.0M;
        }

        private void rad_fix_price_CheckedChanged(object sender, EventArgs e)
        {
            num_fix.Enabled = rad_fix_price.Checked;
            setLabel();
        }
        private void rad_fix_disc_CheckedChanged(object sender, EventArgs e)
        {
            num_disc.Enabled = rad_fix_disc.Checked;
            setLabel();
        }
        private void rad_percentage_CheckedChanged(object sender, EventArgs e)
        {
            num_percentage.Enabled = rad_percentage.Checked;
            setLabel();
        }
        private void setLabel()
        {
            lbl_status.Text = Discount.ToString();
        }
    }
}
