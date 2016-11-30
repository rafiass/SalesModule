using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    class MainViewModel
    {
        public DelegateCommand OpenCommand { get; private set; }

        public MainViewModel()
        {
            OpenCommand = new DelegateCommand(o => openNewSale((SaleTypes)o));
        }

        private void openNewSale(SaleTypes type)
        {
            SaleViewModel svm = null;
            //switch (type)
            //{
            //case SaleTypes.SingularLowerPrice: s = LowPriceProductForm.Create(); break;
            //case SaleTypes.SingularBuyAndGet: s = SingularBuyAndGet.Create(); break;
            //case SaleTypes.Buy2GetAdvanced: s = Buy2GetAdvancedForm.Create(); break;
            //case SaleTypes.AdvancedBundle: s = BundleAdvancedForm.Create(); break;
            //}
            svm = new SomeSaleViewModel();
            if (svm == null)
                return;
            var prop = new PopupProperties();
            //### sale window properties
            InteropService.OpenWindow(svm, prop);
            if (svm.Sale != null)
            {
                if (DBService.GetService().InsertGroup(new SalesGroupM(
                    Wrapper.User, DateTime.Now, true, svm.Sale)) != -1)
                    MessageBox.Show("המבצע נוצר בהצלחה!");
                else
                    MessageBox.Show("אירעה שגיאה בזמן יצירת המבצע אנא פנה אל מרכז התמיכה");
            }
        }
    }
}
