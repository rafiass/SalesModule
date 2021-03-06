﻿using System;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class LowPricedProductAdvViewModel : SaleViewModel
    {
        private bool _isDiscountPerAmount, _isAmountLimited, _isGiftAvailable;

        public string Title { get; set; }

        public IProductM SelectedProduct { get; set; }
        public DiscountM Discount { get; private set; }

        public bool IsDiscountPerAmount
        {
            get { return _isDiscountPerAmount; }
            set { SetProperty(ref _isDiscountPerAmount, value); }
        }
        public double AmountDiscounted { get; set; }
        public bool IsAmountLimited
        {
            get { return _isAmountLimited; }
            set { SetProperty(ref _isAmountLimited, value); }
        }
        public double LimitedAmount { get; set; }
        public bool IsGiftAvailable
        {
            get { return _isGiftAvailable; }
            set { SetProperty(ref _isGiftAvailable, value); }
        }
        public IProductM Gifted { get; set; }

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "מוצר מוזל",
                    Width = 350,
                    Height = 550
                };
            }
        }

        public LowPricedProductAdvViewModel() : this(null) { }
        public LowPricedProductAdvViewModel(SaleM s) : base(s)
        {
        }

        protected override void LoadEmpty()
        {
            Title = "מוצר מוזל";
            SelectedProduct = null;
            Discount = new DiscountM(10, DiscountTypes.Fix_Discount);

            IsDiscountPerAmount = false;
            AmountDiscounted = 1;
            IsAmountLimited = false;
            LimitedAmount = 1;

            IsGiftAvailable = false;
            Gifted = null;
        }
        protected override void LoadSale(SaleM s)
        {
            if (s.Discounted == null || s.Discounted.Count != 1 || s.Discounted[0].Discount == null)
                throw new InvalidOperationException("Discounted product data mismatch.");
            var discounted = s.Discounted[0];
            if (discounted.Gifted == null || discounted.Gifted.Count > 1)
                throw new InvalidOperationException("Gifted product data mismatch.");

            Title = s.Title;
            SelectedProduct = DBService.GetService().GetProduct(discounted.ID, discounted.IsPluno);

            IsDiscountPerAmount = discounted.Amount != 0;
            AmountDiscounted = discounted.Amount != 0 ? discounted.Amount : 1;
            IsAmountLimited = discounted.MaxMultiply != 0;
            LimitedAmount = discounted.MaxMultiply != 0 ? discounted.MaxMultiply : 1;
            IsGiftAvailable = discounted.Gifted.Count > 0;
            if (discounted.Gifted.Count > 0)
                Gifted = DBService.GetService().GetProduct(
                    discounted.Gifted[0].ID, discounted.Gifted[0].IsPluno);
            Discount = discounted.Discount;
        }
        protected override SaleM CreateSale()
        {
            if (Title.Trim() == "")
                throw new SalesException("שם מבצע לא תקין");
            if (SelectedProduct == null)
                throw new SalesException("אנא בחר מוצר למבצע.");
            if (IsGiftAvailable && Gifted == null)
                throw new SalesException("אנא בחר מוצר כמתנה.");

            var outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM(SelectedProduct.ID, SelectedProduct.isPluno,
                IsDiscountPerAmount ? AmountDiscounted : 0,
                IsAmountLimited ? LimitedAmount : 0, Discount,
                !IsGiftAvailable ? null : new GiftedProductM(Gifted,
                    1, new DiscountM(0, DiscountTypes.Fix_Price))));
            _prop.RecurrencePerInstance = 1;
            return new SaleM(Title, SaleTypes.LowPricedProductAdv, _prop,
                null, outs, null, _isEditing ? _index : 1, _ID);
        }
    }
}
