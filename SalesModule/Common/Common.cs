using System;
using System.Collections.Generic;

namespace SalesModule
{
    internal static class Common
    {
        public static void InsertItemToCart(List<ShoppingItem> cart, ShoppingItem newItem)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Price > newItem.Price)
                    cart.Insert(i, newItem);
                else if (cart[i].Price == newItem.Price && cart[i].Pluno == newItem.Pluno)
                    cart[i].Add(newItem);
                else
                    continue;
                return;
            }
            cart.Add(newItem);
        }
        public static ShoppingItem ReduceAmountFromCart(List<ShoppingItem> cart, string ID, bool isPluno, double amount)
        {
            var removeList = cart.FindAll(si =>
                (isPluno && si.Pluno == ID) ||
                (!isPluno && si.Kind != null && si.Kind.ToString() == ID));
            if (removeList.Count == 0) return null;
            else if (removeList.Count == 1)
            {
                double newAmount = removeList[0].Reduce(amount);
                if (newAmount < 0)
                {
                    //### remove more than exist
                    cart.Remove(removeList[0]);
                }
                if (newAmount == 0)
                    cart.Remove(removeList[0]);
                return new ShoppingItem(removeList[0].Pluno, newAmount, removeList[0].Price);
            }
            else// if (removeList.Count > 1)
            {
                //### more than one instance found
                return null;
            }
        }

        public static void collapseSales(List<SaleDiscount> sales)
        {
            for (int i = 0; i < sales.Count; i++)
            {
                if (sales[i].Discount == 0)
                    sales.RemoveAt(i--);
                else if (i < sales.Count - 1)
                    for (int j = i + 1; j < sales.Count; j++)
                        if (sales[i].Add(sales[j]))
                            sales.RemoveAt(j--);
            }
        }
    }

    internal class ShoppingItem
    {
        public string Pluno { get; private set; }
        public int? Kind { get; private set; }
        public double Amount { get; private set; }
        public double Price { get; private set; }

        public ShoppingItem(string pluno, double amount, double price)
            : this(pluno, amount, price, null) { }
        public ShoppingItem(ShoppingItem si)
            : this(si.Pluno, si.Amount, si.Price, si.Kind)
        { }
        public ShoppingItem(string pluno, double amount, double price, int? kind)
        {
            Pluno = pluno;
            Kind = kind;
            Amount = amount;
            Price = price;
        }

        public bool IsTheSameAs(ComperableProduct cp)
        {
            return (cp.isPluno && Pluno == cp.ID) ||
                (!cp.isPluno && Kind != null && Kind.ToString() == cp.ID);
        }

        public override bool Equals(object obj)
        {
            if (obj is ShoppingItem)
            {
                var si = obj as ShoppingItem;
                return Pluno == si.Pluno &&
                    Price == si.Price;
            }
            if (obj is ComperableProduct)
            {
                return IsTheSameAs(obj as ComperableProduct);
            }
            return false;
        }
        public static bool operator ==(ShoppingItem si1, ShoppingItem si2)
        {
            return si1.Equals(si2);
        }
        public static bool operator !=(ShoppingItem si1, ShoppingItem si2)
        {
            return !si1.Equals(si2);
        }
        public static bool operator ==(ShoppingItem si, ComperableProduct cp)
        {
            return si.Equals(cp);
        }
        public static bool operator !=(ShoppingItem si, ComperableProduct cp)
        {
            return !si.Equals(cp);
        }
        public override int GetHashCode()
        {
            try
            {
                return int.Parse(Pluno);
            }
            catch
            {
                return 0;
            }
        }

        public bool Add(ShoppingItem si)
        {
            if (si != this)
                return false;
            if (Kind == null) Kind = si.Kind;
            Amount += si.Amount;
            return true;
        }
        public double Reduce(double amount)
        {
            return Amount -= amount;
        }
    }

    internal class UserData
    {
        public int EmpNo { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public UserData(int empno, string userName, string password)
        {
            EmpNo = empno;
            UserName = userName;
            Password = password;
        }
    }

    internal enum DiscountTypes { Nothing = 1, Percentage, Fix_Price, Fix_Discount };
    internal class Discount
    {
        public double Amount { get; private set; }
        public DiscountTypes Type { get; private set; }
        public Discount(double amount, DiscountTypes type)
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
