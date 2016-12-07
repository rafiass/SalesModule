using System;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    class LowPricedProductViewModel : SaleViewModel
    {
        private double _limitedAmount;

        public IProductM SelectedProduct { get; set; }
        public DiscountM Discount { get; private set; }

        public bool IsDiscountPerAmount { get; set; }
        public double AmountDiscounted { get; set; }
        public bool IsAmountLimited { get; set; }
        public double LimitedAmount { get; set; }
        public bool IsGiftAvailable { get; set; }
        public IProductM Gifted { get; set; }

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "מוצר מוזל",
                    Width = 288,
                    Height = 405,
                    IsModal = true
                };
            }
        }

        public LowPricedProductViewModel() : base() { }
        public LowPricedProductViewModel(SaleM s) : base(s) { }

        protected override void LoadSale()
        {

        }
        protected override void LoadSale(SaleM s)
        {
            _limitedAmount = 0;
        }

        protected override SaleM CreateSale()
        {
            return null;
        }
    }
}
