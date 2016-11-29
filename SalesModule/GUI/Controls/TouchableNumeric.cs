using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesModule.GUI
{
    internal class TouchableNumeric : NumericUpDown
    {
        public string Title { get; set; }
        public TouchableNumeric()
            : base()
        {
            Title = "הכנס ערך";
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ShowNumPad();
        }
        private void ShowNumPad()
        {
            var res = NumPad.GetValue(Title) ?? (double)Value;
            var temp = (decimal)res;
            temp /= Increment;
            var newVal = Math.Round(temp) * Increment;
            if (newVal > Maximum) Value = Maximum;
            else if (newVal < Minimum) Value = Minimum;
            else Value = newVal;
        }
    }
}
