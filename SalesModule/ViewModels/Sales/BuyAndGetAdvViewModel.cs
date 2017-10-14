using System;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class BuyAndGetAdvViewModel : SaleViewModel
    {
        public string Title { get; set; }
        public IProductM BuyProduct { get; set; }
        public double BuyAmount { get; set; }
        public IProductM GetProduct { get; set; }
        public double GetAmount { get; set; }
        public DiscountM GetDiscount { get; private set; }

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "קנה וקבל",
                    Width = 500,
                    Height = 750,
                    MinWidth = 400,
                    MinHeight = 550
                };
            }
        }

        public BuyAndGetAdvViewModel() : this(null) { }
        public BuyAndGetAdvViewModel(SaleM s) : base(s)
        {
        }

        protected override void LoadEmpty()
        {
            Title = "קנה וקבל";
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

            Title = s.Title;
            BuyProduct = DBService.GetService().GetProduct(s.ReqProducts[0].ID, s.ReqProducts[0].IsPluno);
            BuyAmount = s.ReqProducts[0].Amount;
            GetProduct = DBService.GetService().GetProduct(s.Discounted[0].ID, s.Discounted[0].IsPluno);
            GetAmount = s.Discounted[0].MaxMultiply;
            GetDiscount = s.Discounted[0].Discount;
        }
        protected override SaleM CreateSale()
        {
            if (Title.Trim() == "")
                throw new SalesException("שם מבצע לא תקין");
            if (BuyProduct == null)
                throw new SalesException("אנא בחר מוצר לקנייה.");
            if (GetProduct == null)
                throw new SalesException("אנא בחר מוצר לקבלה.");

            var reqs = new List<ProdAmountM>();
            reqs.Add(new ProdAmountM(BuyProduct.ID, BuyProduct.isPluno, BuyAmount));
            var outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM(GetProduct.ID, GetProduct.isPluno, 1, GetAmount, GetDiscount));
            return new SaleM(Title, SaleTypes.BuyAndGetAdv, _prop, reqs, outs, null, _isEditing ? _index : 1, _ID);
        }

        protected override SalesPropertiesM CreateSaleProperties()
        {
            return new SalesPropertiesM() { InstanceMultiply = 0 };
        }
    }
}
