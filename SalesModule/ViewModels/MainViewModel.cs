using System;
using System.Windows;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class MainViewModel
    {
        public DelegateCommand<SaleTypes> OpenCommand { get; private set; }

        public MainViewModel()
        {
            OpenCommand = new DelegateCommand<SaleTypes>(openNewSale);
        }

        private void openNewSale(SaleTypes type)
        {
            var s = SalesFactoryService.CreateSale(type);
            if (s == null) return; // operation canceled

            if (DBService.GetService().InsertGroup(new SalesGroupM(Wrapper.User, DateTime.Now, true, s)) != -1)
                MessageBox.Show("המבצע נוצר בהצלחה!");
            else
                MessageBox.Show("אירעה שגיאה בזמן יצירת המבצע אנא פנה אל מרכז התמיכה");
        }
    }
}
