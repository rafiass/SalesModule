using System;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class BuyAndGetViewModel : SaleViewModel
    {
        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "קנה וקבל",
                    Width = 500,
                    Height = 700
                };
            }
        }

        public IProductM BuyProduct { get; set; }
        public double BuyAmount { get; set; }
        public IProductM GetProduct { get; set; }
        public double GetAmount { get; set; }
        public DiscountM GetDiscount { get; private set; }

        public BuyAndGetViewModel() : base() { }
        public BuyAndGetViewModel(SaleM s) : base(s) { }

        protected override void LoadSale()
        {
            BuyProduct = null;
            BuyAmount = 1;
            GetProduct = null;
            GetAmount = 1;
            GetDiscount = new DiscountM(0, DiscountTypes.Fix_Price);
        }
        protected override void LoadSale(SaleM s)
        {
            if (s.ReqProducts == null || s.ReqProducts.Count != 1)
                throw new SalesException("Required's product data mismatch.");
            if (s.Discounted == null || s.Discounted.Count != 1)
                throw new SalesException("Discounted's product data mismatch.");

            BuyProduct = DBService.GetService().GetProduct(s.ReqProducts[0].ID, s.ReqProducts[0].isPluno);
            BuyAmount = s.ReqProducts[0].Amount;
            GetProduct = DBService.GetService().GetProduct(s.Discounted[0].ID, s.Discounted[0].isPluno);
            GetAmount = s.Discounted[0].MaxMultiply;
            GetDiscount = s.Discounted[0].Discount;
        }
        protected override SaleM CreateSale()
        {
            if (BuyProduct == null)
                throw new SalesException("אנא בחר מוצר לקנייה.");
            if (GetProduct == null)
                throw new SalesException("אנא בחר מוצר לקבלה.");

            var reqs = new List<ProdAmountM>();
            reqs.Add(new ProdAmountM(BuyProduct.ID, BuyProduct.isPluno, BuyAmount));
            var outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM(GetProduct.ID, GetProduct.isPluno, 1, GetAmount, GetDiscount));
            return new SaleM(SaleTypes.SingularBuyAndGet, _prop, reqs, outs, null, _isEditing ? _index : 1, _ID);
        }

        protected override SalesPropertiesM CreateSaleProperties()
        {
            return new SalesPropertiesM("קנה וקבל") { InstanceMultiply = 0 };
        }
    }
}
