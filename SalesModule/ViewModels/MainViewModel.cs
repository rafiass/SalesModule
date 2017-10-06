using System;
using System.Windows;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class MainViewModel
    {
        public DelegateCommand<SaleTypes> OpenCommand { get; private set; }
        public DelegateCommand ManagementCommand { get; private set; }

        public DelegateCommand TestCommand { get; private set; }

        public MainViewModel()
        {
            OpenCommand = new DelegateCommand<SaleTypes>(openNewSale);
            ManagementCommand = new DelegateCommand(openManagement);
            TestCommand = new DelegateCommand(() => InteropService.OpenWindow(new TestViewModel()));
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

        private void openManagement()
        {
            InteropService.OpenWindow(new SalesManagementViewModel());
        }
    }
}
