using SalesModule.Models;
using SalesModule.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesModule.ViewModels
{
    internal class BuyAndGetViewModel : SaleViewModel
    {
        public string Title { get; set; }

        public IProductM SelectedProduct { get; set; }
        public double BuyAmount { get; set; }
        public double GetAmount { get; set; }

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "קנה וקבל",
                    Width = 600,
                    Height = 600
                };
            }
        }

        public BuyAndGetViewModel() : this(null) { }
        public BuyAndGetViewModel(SaleM s) : base(s)
        {
        }

        protected override void LoadEmpty()
        {
            Title = "קנה וקבל";
            SelectedProduct = null;
            BuyAmount = 1;
            GetAmount = 1;
        }
        protected override void LoadSale(SaleM s)
        {
            if (s.ReqProducts == null || s.ReqProducts.Count != 1 ||
                s.Discounted == null || s.Discounted.Count != 1 ||
                s.ReqProducts[0] != s.Discounted[0])
                throw new SalesException("Selected product data mismatch.");

            Title = s.Title;
            SelectedProduct = DBService.GetService().GetProduct(s.ReqProducts[0].ID, s.ReqProducts[0].IsPluno);
            BuyAmount = s.ReqProducts[0].Amount;
            GetAmount = s.Discounted[0].MaxMultiply;
        }
        protected override SaleM CreateSale()
        {
            if (Title.Trim() == "")
                throw new SalesException("שם מבצע לא תקין");
            if (SelectedProduct == null)
                throw new SalesException("אנא בחר מוצר לקנייה.");

            var reqs = new List<ProdAmountM>();
            reqs.Add(new ProdAmountM(SelectedProduct.ID, SelectedProduct.isPluno, BuyAmount));
            var outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM(SelectedProduct.ID, SelectedProduct.isPluno, 1, GetAmount, new DiscountM(0, DiscountTypes.Fix_Price)));
            return new SaleM(Title, SaleTypes.SingularBuyAndGet, _prop, reqs, outs, null, _isEditing ? _index : 1, _ID);
        }

        protected override SalesPropertiesM CreateSaleProperties()
        {
            return new SalesPropertiesM() { InstanceMultiply = 0 };
        }
    }
}
