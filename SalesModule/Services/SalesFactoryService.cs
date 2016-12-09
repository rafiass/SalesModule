using System;
using System.Windows;
using SalesModule.Models;
using SalesModule.ViewModels;

namespace SalesModule.Services
{
    internal static class SalesFactoryService
    {
        public static SaleM CreateSale(SaleTypes type)
        {
            ActivityLogService.Logger.LogCall();
            SaleViewModel svm;
            switch (type)
            {
                case SaleTypes.TargetPrice: svm = null; break;
                case SaleTypes.SingularLowerPrice: svm = new LowPricedProductViewModel(); break;
                case SaleTypes.SingularBuyAndGet: svm = new BuyAndGetViewModel(); break;
                case SaleTypes.Bundle: svm = null; break;
                default: svm = null; break;
            }
            if (svm == null)
                throw new ArgumentException("לא ניתן ליצור את המבצע הנדרש");

            InteropService.OpenWindow(svm, svm.PopupProperties);
            return svm.Conducted;
        }
        public static SaleM EditSale(SaleM sale)
        {
            ActivityLogService.Logger.LogCall();
            try
            {
                SaleViewModel svm;
                switch (sale.Type)
                {
                    case SaleTypes.TargetPrice: svm = null; break;
                    case SaleTypes.SingularLowerPrice: svm = new LowPricedProductViewModel(sale); break;
                    case SaleTypes.SingularBuyAndGet: svm = new BuyAndGetViewModel(sale); break;
                    case SaleTypes.Bundle: svm = null; break;
                    default: svm = null; break;
                }
                if (svm == null)
                    throw new ArgumentException("לא ניתן לערוך את המבצע הקיים");

                InteropService.OpenWindow(svm, svm.PopupProperties);
                return svm.Conducted;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אירעה שגיאה בעת פתיחת מבצע לעריכה. המידע פגום.");
                return null;
            }
        }
    }
}
