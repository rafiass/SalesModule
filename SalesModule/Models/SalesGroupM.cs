using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesModule.Models
{
    internal class SalesGroupM
    {
        public int GroupID { get; private set; }
        public UserData Emp { get; private set; }
        public DateTime DateCreated { get; private set; }
        public bool IsEnabled { get; private set; }

        public List<SaleM> Sales { get; set; }

        public SalesGroupM(SaleM sale)
            : this(null, DateTime.Now, true, sale)
        { }
        public SalesGroupM(UserData emp, DateTime created, bool isEnabled, SaleM sale)
            : this(-1, emp, created, isEnabled)
        {
            Sales = new List<SaleM>();
            Sales.Add(sale);
        }
        public SalesGroupM(int groupID, UserData emp, DateTime created, bool isEnabled, List<SaleM> sales = null)
        {
            GroupID = groupID;
            Sales = sales ?? new List<SaleM>();
            Emp = emp;
            DateCreated = created;
            IsEnabled = isEnabled;
        }

        public List<SaleDiscount> GetEfective(List<ShoppingItem> bag, double totalReceipt)
        {
            List<SaleDiscount> sd;
            foreach(var sale in Sales)
                if ((sd = sale.GetEfective(bag, totalReceipt)) != null)
                    return sd;
            return null;
        }

        public string IsProductRequire(string pluno, int? kind)
        {
            var sl = Sales.Find(s => s.IsProductRequire(pluno, kind));
            return sl?.Title ?? "";
        }
    }
}
