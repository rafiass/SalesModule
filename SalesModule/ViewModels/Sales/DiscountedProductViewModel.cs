using System;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class DiscountedProductViewModel : SaleViewModel
    {
        public string Title { get; set; }

        public IProductM SelectedProduct { get; set; }
        public DiscountM Discount { get; set; }

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "מוצר בהנחה",
                    Width = 450,
                    Height = 530,
                    MinWidth = 400,
                    MinHeight = 350
                };
            }
        }

        public DiscountedProductViewModel() : this(null) { }
        public DiscountedProductViewModel(SaleM s) : base(s)
        {
        }

        protected override void LoadEmpty()
        {
            Title = "מוצר מוזל";
            SelectedProduct = null;
            Discount = new DiscountM(15, DiscountTypes.Percentage);
        }
        protected override void LoadSale(SaleM s)
        {
            if (s.Discounted == null || s.Discounted.Count != 1 || s.Discounted[0].Discount == null)
                throw new InvalidOperationException("Discounted product data mismatch.");
            var discounted = s.Discounted[0];

            Title = s.Title;
            SelectedProduct = DBService.GetService().GetProduct(discounted.ID, discounted.IsPluno);
            Discount = discounted.Discount;
        }
        protected override SaleM CreateSale()
        {
            if (Title.Trim() == "")
                throw new SalesException("שם מבצע לא תקין");
            if (SelectedProduct == null)
                throw new SalesException("אנא בחר מוצר למבצע.");

            var outs = new List<DiscountedProductM>
            { new DiscountedProductM(SelectedProduct.ID, SelectedProduct.isPluno, 0, 0, Discount) };
            _prop.RecurrencePerInstance = 1;
            return new SaleM(Title, SaleTypes.DiscountedProduct, _prop,
                null, outs, null, _isEditing ? _index : 1, _ID);
        }
    }
}
