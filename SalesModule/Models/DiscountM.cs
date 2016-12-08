
using SalesModule.ViewModels;
namespace SalesModule.Models
{
    internal enum DiscountTypes { Nothing = 1, Percentage, Fix_Price, Fix_Discount };
    internal class DiscountM : ViewModelBase
    {
        private double _amount;
        private DiscountTypes _type;

        public double Amount
        {
            get { return _amount; }
            set
            {
                if (SetProperty(ref _amount, value))
                    OnPropertyChanged("Summary");
            }
        }
        public DiscountTypes Type
        {
            get { return _type; }
            set
            {
                if (SetProperty(ref _type, value))
                    OnPropertyChanged("Summary");
            }
        }
        public string Summary { get { return ToString(); } }

        public DiscountM(double amount, DiscountTypes type)
        {
            _amount = amount;
            _type = type;
        }

        public double GetDiscount(double productPrice)
        {
            return GetDiscount(productPrice, productPrice);
        }
        public double GetDiscount(double saleValue, double receiptValue)
        {
            switch (Type)
            {
                case DiscountTypes.Percentage:
                    return Amount < 0 || Amount > 100 ? 0 : Amount * receiptValue / 100;
                case DiscountTypes.Fix_Price:
                    return Amount < 0 || saleValue < Amount ? 0 : saleValue - Amount;
                case DiscountTypes.Fix_Discount:
                    return Amount < 0 ? 0 : Amount;
                case DiscountTypes.Nothing:
                default: return 0;
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case DiscountTypes.Percentage:
                    return "בהנחה של " + Amount + "%";
                case DiscountTypes.Fix_Price:
                    return "במחיר חדש של " + Amount + "₪";
                case DiscountTypes.Fix_Discount:
                    return "בהנחה של " + Amount + "₪";
                case DiscountTypes.Nothing:
                default: return "ללא הנחה";
            }
        }
    }
}
