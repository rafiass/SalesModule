using SalesModule.Models;

namespace SalesModule.ViewModels
{
    internal class SalesPropertiesViewModel : PopupViewModel
    {
        //### SalesPropertiesViewModel

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "הגדרות מבצע",
                    Width = 282,
                    Height = 357,
                    IsModal = true
                };
            }
        }

        private SalesPropertiesM _assembled;

        public SalesPropertiesM Conducted { get; private set; }
        public bool DatesEnabled { get; set; }

        public DelegateCommand CommitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public SalesPropertiesViewModel()
            : this(null)
        {
        }
        public SalesPropertiesViewModel(SalesPropertiesM prop)
        {
            CommitCommand = new DelegateCommand(CommitFunc);
            CancelCommand = new DelegateCommand(CancelFunc);

            _assembled = prop;
        }

        private void CommitFunc()
        {
            Conducted = null;//### SalesPropertiesViewModel
            CloseWindow();
        }
        private void CancelFunc()
        {
            //### SalesPropertiesViewModel: are you sure you want to quit?
            Conducted = _assembled;
            CloseWindow();
        }
        protected internal override void WindowClosed()
        {
            CancelFunc();
        }
    }
}