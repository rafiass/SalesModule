using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesModule;

namespace SalesTester
{
    class CashierMock : ICashier
    {
        private ISalesEngine _engine;

        public List<CashierItem> Items { get; private set; }
        public CashierMock()
        {
            Items = new List<CashierItem>();
            _engine = Wrapper.CreateEngine();
            _engine.EngineRestarted += engineResetarted;
            _engine.SaleApplied += saleApplied;
            _engine.SaleCancelled += saleCancelled;
            resetCart();
        }

        public void addProduct(CashierItem ci)
        {
            addProduct(ci.ID, ci.QTY, ci.Price);
        }
        public void addProduct(string pluno, double qty, double price)
        {
            Items.Add(new CashierItem(pluno, pluno, qty, price, CashierItemType.Product));
            _engine.AddItem(pluno, qty, price);
        }
        public void resetCart()
        {
            _engine.InitializeForDebugging();
        }
        public void removeProduct(string pluno, double qty)
        {
            var l = Items.FindAll(p => p.ID == pluno && p.Type == CashierItemType.Product);
            if (l.Count != 1)
                throw new ArgumentException("Product not found to be removed (id = " + pluno + ")");
            else if (l[0].QTY < qty)
                throw new ArgumentException("Not enough items to remove (id = " + qty + ")");
            else if (l[0].QTY == qty)
                Items.Remove(l[0]);
            else
                l[0].QTY -= qty;

            _engine.RemoveItem(pluno, qty);
        }

        private void engineResetarted()
        {
            Items.Clear();
        }
        private void saleApplied(ISaleDiscount s)
        {
            Items.Add(new CashierItem(s.Title, s.ID.ToString(), s.Quantity, s.Discount, CashierItemType.Discount));
        }
        private void saleCancelled(int id)
        {
            var l = Items.FindAll(s => s.Type == CashierItemType.Discount && s.ID == id.ToString());
            if (l.Count != 1)
                throw new InvalidOperationException("Sale to be removed not found (id = " + id + ")");
            Items.RemoveAll(s => s.Type == CashierItemType.Discount && s.ID == id.ToString());
        }
    }
}
