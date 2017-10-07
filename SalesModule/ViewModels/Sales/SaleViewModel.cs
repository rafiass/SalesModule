using System;
using System.ComponentModel;
using System.Windows;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal abstract class SaleViewModel : PopupViewModel
    {
        protected bool _isEditing { get { return _index != -1; } }
        protected int _index { get; private set; }
        protected int _ID { get; private set; }
        protected SalesPropertiesM _prop { get; private set; }

        public SaleM Conducted { get; private set; }

        public DelegateCommand PropertiesCommand { get; private set; }
        public DelegateCommand CommitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        protected SaleViewModel()
            : this(null)
        {
        }
        protected SaleViewModel(SaleM s)
        {
            _index = s != null ? s.Index : -1;
            _ID = s != null ? s.SaleID : -1;
            _prop = s != null ? s.Properties :
                CreateSaleProperties();


            SetPopupTitle("מוצר במבצע");

            PropertiesCommand = new DelegateCommand(propertiesFunc);
            CommitCommand = new DelegateCommand(CommitFunc);
            CancelCommand = new DelegateCommand(CancelFunc);

            if (s != null) LoadSale(s);
            else LoadEmpty();
        }

        protected abstract void LoadEmpty();
        protected abstract void LoadSale(SaleM s);
        protected abstract SaleM CreateSale();

        protected virtual SalesPropertiesM CreateSaleProperties()
        {
            return new SalesPropertiesM();
        }
        protected virtual SalesPropertiesViewModel CreatePropertiesSettings()
        {
            return new SalesPropertiesViewModel(_prop) { IsDatesEnabled = !_isEditing };
        }

        private void propertiesFunc()
        {
            var propVM = CreatePropertiesSettings();
            InteropService.OpenWindow(propVM);
            _prop = propVM.Conducted ?? _prop;
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
            if (MessageBox.Show("שינויים שעשית לא נשמרו. האם אתה בטוח שברצונך לצאת?",
                "ביטול שינויים", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            Conducted = null;
            CloseWindow();
        }
        protected internal override void WindowClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            if (MessageBox.Show("שינויים שעשית לא נשמרו. האם אתה בטוח שברצונך לצאת?",
                "ביטול שינויים", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            Conducted = null;
        }
    }
}
