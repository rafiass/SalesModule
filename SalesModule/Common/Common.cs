using System;
using System.Collections.Generic;
using SalesModule.Models;

namespace SalesModule
{
    internal enum SaleTypes
    {
        None = 0, TargetPrice, LowPricedProductAdv,
        SingularBuyAndGet, BuyAndGetAdv, Bundle
    }

    internal enum SearchOrder
    {
        lowToHigh, highToLow
    }

    internal static class Common
    {
        public static string CurrentDirectory { get { return Environment.CurrentDirectory; } }

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
}
