using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTester
{
    interface ICashier
    {
        List<CashierItem> Items { get; }

        void addProduct(CashierItem ci);
        void addProduct(string pluno, double qty, double price);
        void resetCart();
        void removeProduct(string id, double amount);
    }

    enum CashierItemType { Product, Discount }
    class CashierItem
    {
        public string Title { get; private set; }
        public string ID { get; private set; }
        public double QTY { get; set; }
        public double Price { get; private set; }
        public CashierItemType Type { get; private set; }

        public CashierItem(string title, string pluno, double qty, double price, CashierItemType type)
        {
            Title = title;
            ID = pluno;
            QTY = qty;
            Price = price;
            Type = type;
        }
    }
}
