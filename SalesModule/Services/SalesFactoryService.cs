using System;
using System.Windows;
using SalesModule.Models;
using SalesModule.ViewModels;
using System.Collections.Generic;

namespace SalesModule.Services
{
    internal static class SalesFactoryService
    {
        private static Dictionary<SaleTypes, Type> s_salesMap;
        static SalesFactoryService()
        {
            s_salesMap = new Dictionary<SaleTypes, Type>
            {
                { SaleTypes.DiscountedProduct,      typeof(DiscountedProductViewModel)},
                { SaleTypes.FixedPricedProduct,     typeof(FixedPricedProductViewModel)},
                { SaleTypes.LowPricedProductAdv,    typeof(LowPricedProductAdvViewModel)},
                { SaleTypes.SingularBuyAndGet,      typeof(BuyAndGetViewModel)},
                { SaleTypes.BuyAndGetAdv,           typeof(BuyAndGetAdvViewModel)}
            };
        }

        public static SaleM CreateSale(SaleTypes type)
        {
            ActivityLogService.Logger.LogFunctionCall(type);
            if (s_salesMap[type] == null)
                throw new ArgumentException("לא ניתן ליצור את המבצע הנדרש");

            var vm = Activator.CreateInstance(s_salesMap[type]);

            if (vm == null || !(vm is SaleViewModel))
                throw new ArgumentException("לא ניתן ליצור את המבצע הנדרש");

            var svm = vm as SaleViewModel;
            InteropService.OpenWindow(svm);

            return svm.Conducted;
        }
        public static SaleM EditSale(SaleM sale)
        {
            ActivityLogService.Logger.LogFunctionCall(sale);
            if (sale == null) return null;
            try
            {
                if (s_salesMap[sale.Type] == null)
                    throw new ArgumentException("לא ניתן ליצור את המבצע הנדרש");

                var vm = Activator.CreateInstance(s_salesMap[sale.Type], sale);

                if (vm == null || !(vm is SaleViewModel))
                    throw new ArgumentException("לא ניתן לערוך את המבצע הנדרש");

                var svm = vm as SaleViewModel;
                InteropService.OpenWindow(svm);

                return svm.Conducted;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                throw new InvalidOperationException("אירעה שגיאה בעת עריכת המבצע. המידע פגום.", ex);
            }
        }
    }
}
