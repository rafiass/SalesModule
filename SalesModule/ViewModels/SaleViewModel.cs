using System;
using System.Windows;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal abstract class SaleViewModel : PopupViewModel
    {
        private bool _isEditing { get { return _index != -1; } }
        private int _index;
        private int _ID;
        private SalesPropertiesM _prop;
        private SaleM _assembled;

        public SaleM Conducted { get; private set; }
        public abstract PopupProperties PopupProperties { get; }

        public DelegateCommand PropertiesCommand { get; private set; }
        public DelegateCommand CommitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        protected SaleViewModel()
            : this(null)
        {
        }
        protected SaleViewModel(SaleM s)
        {
            _assembled = s;
            _index = s != null ? s.Index : -1;
            _ID = s != null ? s.SaleID : -1;
            _prop = s != null ? s.Properties :
                new SalesPropertiesM("מוצר במבצע");

            PropertiesCommand = new DelegateCommand(propertiesFunc);
            CommitCommand = new DelegateCommand(CommitFunc);
            CancelCommand = new DelegateCommand(CancelFunc);

            if (s == null) LoadSale(s);
            else LoadSale();
        }

        protected abstract void LoadSale();
        protected abstract void LoadSale(SaleM s);
        protected abstract SaleM CreateSale();

        private void propertiesFunc()
        {
            var propVM = new SalesPropertiesViewModel(_prop);
            //prop.RecurrenceEnabled = false;
            propVM.DatesEnabled = !_isEditing;

            InteropService.OpenWindow(propVM, SalesPropertiesViewModel.PopupProperties);
            _prop = propVM.Conducted;
        }

        private void CommitFunc()
        {
            try
            {
                Conducted = CreateSale();
                CloseWindow();
            }
            catch (SalesException ex)
            {
                MessageBox.Show(ex.Message, "נתונים שגויים", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אירעה שגיאה בעת יצירת המבצע, אנא פנה אל מרכז התמיכה.", "שגיאה");
            }
        }
        private void CancelFunc()
        {
            //### SaleViewModel: are you sure you want to quit?
            Conducted = _assembled;
            CloseWindow();
        }
        protected internal override void WindowClosed()
        {
            CancelFunc();
        }
    }
}
