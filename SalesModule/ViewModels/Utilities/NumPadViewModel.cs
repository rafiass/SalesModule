using System.ComponentModel;
using System.Windows;
using SalesModule.Models;

namespace SalesModule.ViewModels
{
    internal class NumPadViewModel : PopupViewModel
    {
        private double _val;
        private string _valstr;

        internal double? Results { get; private set; }
        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "בחר ערך",
                    Width = 348,
                    Height = 529
                };
            }
        }

        public string Title { get; private set; }
        public double Value
        {
            get { return _val; }
            set { SetProperty(ref _val, value); }
        }

        public DelegateCommand<string> DigitCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand OkCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public NumPadViewModel(string title, double val = 0.0)
        {
            DigitCommand = new DelegateCommand<string>(digitFunction);
            ClearCommand = new DelegateCommand(clearFunction);
            OkCommand = new DelegateCommand(okFunction);
            CancelCommand = new DelegateCommand(cancelFunction);

            Title = title;
            Value = val;
            _valstr = "0";
            Results = null;
        }

        private void digitFunction(string newDigit)
        {
            double tempVal;
            var append = newDigit + (newDigit == "." ? "0" : "");
            if (!double.TryParse(_valstr + append, out tempVal)) return;

            Value = tempVal;
            _valstr += newDigit;
        }
        private void clearFunction()
        {
            Value = 0.0;
            _valstr = "0";
        }
        private void okFunction()
        {
            Results = Value;
            CloseWindow();
        }
        private void cancelFunction()
        {
            if (Value != 0.0 && MessageBox.Show("שינויים שעשית לא נשמרו. האם אתה בטוח שברצונך לצאת?",
                "ביטול שינויים", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            Results = null;
            CloseWindow();
        }
        protected internal override void WindowClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            if (Value != 0.0 && MessageBox.Show("שינויים שעשית לא נשמרו. האם אתה בטוח שברצונך לצאת?",
                "ביטול שינויים", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            Results = null;
        }
    }
}
