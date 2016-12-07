
namespace SalesModule.Models
{
    internal enum DiscountTypes { Nothing = 1, Percentage, Fix_Price, Fix_Discount };
    internal class DiscountM
    {
        public double Amount { get; private set; }
        public DiscountTypes Type { get; private set; }

        public DiscountM(double amount, DiscountTypes type)
        {
            Amount = amount;
            Type = type;
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
