using System;
using System.Windows.Forms;

namespace SalesModule.GUI
{
    public partial class NumPad : Form
    {
        private string ValueStr;
        private double Value;
        private NumPad(string title)
        {
            InitializeComponent();
            Value = 0.0;
            ValueStr = "0";
            lbl_title.Text = title;
        }
        public static double? GetValue(string Title)
        {
            var win = new NumPad(Title);
            if (win.ShowDialog() == DialogResult.OK)
                return win.Value;
            return null;
        }

        private void Num_click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            double tempVal;
            if (btn == null) return;

            var newchar = btn.Text;
            if (newchar == ".")
                newchar += "0";
            if (double.TryParse(ValueStr + newchar, out tempVal))
            {
                Value = tempVal;
                ValueStr += btn.Text;
            }
            txt_value.Text = Value.ToString();
            btn_ok.Focus();
        }
        private void btn_clear_Click(object sender, EventArgs e)
        {
            Value = 0.0;
            ValueStr = "0";
            txt_value.Text = "0";
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        private void btn_cncl_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
