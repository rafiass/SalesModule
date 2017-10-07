using System;
using System.ComponentModel;
using System.Windows;
using SalesModule.Models;

namespace SalesModule.ViewModels
{
    internal class SalesPropertiesViewModel : PopupViewModel
    {
        private bool _isMaxEnabled, _isMultiplyEnabled, _isRecurrenceEnabled, _isDatesEnabled;
        private bool _isPriceLimited, _isMultiplyLimited, _isRecurrenceLimited;
        private double _maxPrice;
        private int _multiply, _recerrences;
        private bool _isBroadSale, _isSaleDatesLimited;

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "הגדרות מבצע",
                    Width = 300,
                    Height = 400
                };
            }
        }

        public SalesPropertiesM Conducted { get; private set; }
        public double MinPrice { get; set; }

        public bool IsMaxEnabled
        {
            get { return _isMaxEnabled; }
            set
            {
                if (SetProperty(ref _isMaxEnabled, value) && !value)
                    IsPriceLimited = false;
            }
        }
        public bool IsPriceLimited
        {
            get { return _isPriceLimited; }
            set { if (SetProperty(ref _isPriceLimited, value)) MaxPrice = 1000; }
        }
        public double MaxPrice
        {
            get { return _maxPrice; }
            set { SetProperty(ref _maxPrice, value); }
        }

        public bool IsMultiplyEnabled
        {
            get { return _isMultiplyEnabled; }
            set
            {
                if (SetProperty(ref _isMultiplyEnabled, value) && !value)
                    IsMultiplyLimited = false;
            }
        }
        public bool IsMultiplyLimited
        {
            get { return _isMultiplyLimited; }
            set { if (SetProperty(ref _isMultiplyLimited, value)) InstanceMultiply = 1; }
        }
        public int InstanceMultiply
        {
            get { return _multiply; }
            set { SetProperty(ref _multiply, value); }
        }

        public bool IsRecurrenceEnabled
        {
            get { return _isRecurrenceEnabled; }
            set
            {
                if (SetProperty(ref _isRecurrenceEnabled, value) && !value)
                    IsRecurrenceLimited = false;
            }
        }
        public bool IsRecurrenceLimited
        {
            get { return _isRecurrenceLimited; }
            set { if (SetProperty(ref _isRecurrenceLimited, value)) RecurrencePerInstance = 1; }
        }
        public int RecurrencePerInstance
        {
            get { return _recerrences; }
            set { SetProperty(ref _recerrences, value); }
        }

        public bool IsDatesEnabled
        {
            get { return _isDatesEnabled; }
            set
            {
                if (SetProperty(ref _isDatesEnabled, value) && !value)
                    IsBroadSale = false;
            }
        }
        public bool IsBroadSale
        {
            get { return _isBroadSale; }
            set { SetProperty(ref _isBroadSale, value); }
        }
        public DateTime DateFrom { get; set; }
        public bool IsSaleDatesLimited
        {
            get { return _isSaleDatesLimited; }
            set { SetProperty(ref _isSaleDatesLimited, value); }
        }
        public DateTime DateTo { get; set; }

        public DelegateCommand CommitCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public SalesPropertiesViewModel(SalesPropertiesM prop)
        {
            CommitCommand = new DelegateCommand(CommitFunc);
            CancelCommand = new DelegateCommand(CancelFunc);

            IsMaxEnabled = IsMultiplyEnabled = IsRecurrenceEnabled = IsDatesEnabled = true;
            populate(prop);
        }

        private void populate(SalesPropertiesM prop)
        {
            MinPrice = prop.MinPrice;

            IsPriceLimited = prop.MaxPrice != null;
            if (IsPriceLimited)
                MaxPrice = (double)prop.MaxPrice;
            IsMultiplyLimited = prop.InstanceMultiply != 0;
            if (IsMultiplyLimited)
                InstanceMultiply = prop.InstanceMultiply;
            IsRecurrenceLimited = prop.RecurrencePerInstance != 0;
            if (IsRecurrenceLimited)
                RecurrencePerInstance = prop.RecurrencePerInstance;

            IsBroadSale = prop.IsBroadSale;
            DateFrom = prop.DateFrom;
            DateTo = prop.DateTo ?? DateFrom + new TimeSpan(7, 0, 0, 0);
            IsSaleDatesLimited = prop.DateTo == null;
        }

        private void CommitFunc()
        {
            if (IsPriceLimited && MaxPrice <= MinPrice)
                throw new SalesException("ערכי מינימום ומקסימום של שווי הסל לא מתאימים.");
            if (IsBroadSale && IsSaleDatesLimited &&
                (DateTo < DateFrom || DateTo < DateTime.Now))
                throw new SalesException("טווח התאריכים אינו תקין");

            Conducted = new SalesPropertiesM(MinPrice,
                IsPriceLimited ? MaxPrice : (double?)null,
                IsMultiplyLimited ? InstanceMultiply : 0,
                IsRecurrenceLimited ? RecurrencePerInstance : 0,
                IsBroadSale, DateFrom, IsSaleDatesLimited ? DateTo : (DateTime?)null);
            CloseWindow();
        }
        private void CancelFunc()
        {
            if (MessageBox.Show("שינויים שעשית לא נשמרו. האם אתה בטוח שברצונך לצאת?",
                  "ביטול שינויים", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            Conducted = null;
            CloseWindow();
        }
        protected internal override void WindowClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            if (MessageBox.Show("שינויים שעשית לא נשמרו. האם אתה בטוח שברצונך לצאת?",
                "ביטול שינויים", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            Conducted = null;
        }
    }
}
