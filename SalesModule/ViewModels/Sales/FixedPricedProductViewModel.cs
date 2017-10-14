using System;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class FixedPricedProductViewModel : SaleViewModel
    {
        public string Title { get; set; }

        public IProductM SelectedProduct { get; set; }
        public double BuyAmount { get; set; }
        public double BuyingPrice { get; set; }

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "מוצר מוזל",
                    Width = 450,
                    Height = 450,
                    MinWidth = 400,
                    MinHeight = 300
                };
            }
        }

        public FixedPricedProductViewModel() : this(null) { }
        public FixedPricedProductViewModel(SaleM s) : base(s)
        {
        }

        protected override void LoadEmpty()
        {
            Title = "מוצר מוזל";
            SelectedProduct = null;
            BuyAmount = 1;
            BuyingPrice = 10;
        }
        protected override void LoadSale(SaleM s)
        {
            if (s.Discounted == null || s.Discounted.Count != 1 || s.Discounted[0].Discount == null)
                throw new InvalidOperationException("Discounted product data mismatch.");
            var discounted = s.Discounted[0];

            Title = s.Title;
            SelectedProduct = DBService.GetService().GetProduct(discounted.ID, discounted.IsPluno);

            BuyAmount = discounted.Amount;
            BuyingPrice = discounted.Discount.Amount;
        }
        protected override SaleM CreateSale()
        {
            if (Title.Trim() == "")
                throw new SalesException("שם מבצע לא תקין");
            if (SelectedProduct == null)
                throw new SalesException("אנא בחר מוצר למבצע.");

            var outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM(SelectedProduct.ID, SelectedProduct.isPluno,
                BuyAmount, 0, new DiscountM(BuyingPrice, DiscountTypes.Fix_Price)));
            _prop.RecurrencePerInstance = 1;
            return new SaleM(Title, SaleTypes.FixedPricedProduct, _prop,
                null, outs, null, _isEditing ? _index : 1, _ID);
        }
    }
}
