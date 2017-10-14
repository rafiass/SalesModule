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

        public static SaleM CreateSale_old(SaleTypes type)
        {
            ActivityLogService.Logger.LogFunctionCall(type);
            SaleViewModel svm;
            switch (type)
            {
                case SaleTypes.DiscountedProduct: svm = new DiscountedProductViewModel(); break;
                case SaleTypes.LowPricedProductAdv: svm = new LowPricedProductAdvViewModel(); break;
                case SaleTypes.FixedPricedProduct: svm = new FixedPricedProductViewModel(); break;
                case SaleTypes.SingularBuyAndGet: svm = new BuyAndGetViewModel(); break;
                case SaleTypes.BuyAndGetAdv: svm = new BuyAndGetAdvViewModel(); break;
                default: throw new ArgumentException("לא ניתן ליצור את המבצע הנדרש");
            }   

            InteropService.OpenWindow(svm);
            return svm.Conducted;
        }
        public static SaleM EditSale_old(SaleM sale)
        {
            ActivityLogService.Logger.LogFunctionCall(sale);
            try
            {
                SaleViewModel svm;
                switch (sale.Type)
                {
                    case SaleTypes.DiscountedProduct: svm = new DiscountedProductViewModel(sale); break;
                    case SaleTypes.LowPricedProductAdv: svm = new LowPricedProductAdvViewModel(sale); break;
                    case SaleTypes.FixedPricedProduct: svm = new FixedPricedProductViewModel(sale); break;
                    case SaleTypes.SingularBuyAndGet: svm = new BuyAndGetViewModel(sale); break;
                    case SaleTypes.BuyAndGetAdv: svm = new BuyAndGetAdvViewModel(sale); break;
                    default: throw new ArgumentException("לא ניתן לערוך את המבצע הקיים");
                }
            
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
