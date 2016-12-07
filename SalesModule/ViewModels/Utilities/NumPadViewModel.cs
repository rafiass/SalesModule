
namespace SalesModule.ViewModels
{
    internal class NumPadViewModel
    {
        public double Value { get; set; }

        public DelegateCommand<char> DigitCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand OkCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public NumPadViewModel()
            : this(0.0)
        {

        }
        public NumPadViewModel(double val)
        {
            DigitCommand = new DelegateCommand<char>(digitFunction);
            ClearCommand = new DelegateCommand(clearFunction);
            OkCommand = new DelegateCommand(okFunction);
            CancelCommand = new DelegateCommand(cancelFunction);
        }

        private void digitFunction(char d)
        {

        }
        private void clearFunction()
        {

        }
        private void okFunction()
        {

        }
        private void cancelFunction()
        {

        }
    }
}
