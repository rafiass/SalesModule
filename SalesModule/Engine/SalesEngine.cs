using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SalesModule
{
    public delegate void RestartHandler();
    public delegate void AppliedHandler(SaleDiscount sd);
    public delegate void CancelHandler(int id);

    [Guid(SalesEngine.EventsId), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISalesEngineEvents
    {
        void EngineRestarted();
        void SaleApplied(SaleDiscount sd);
        void SaleCancelled(int id);
    }

    [Guid(SalesEngine.InterfaceId)]
    public interface ISalesEngine
    {
        bool Initialized { get; }

        bool Initialize(string vipno = null);

        string AddItem(string pluno, double qty, double price);
        string AddItem(string pluno, double qty, double price, int kind);
        void RemoveItem(string pluno, double qty);

        bool IsPlunoActive(string pluno);
        SaleDiscount[] CalcAllSales();
    }

    [Guid(ClassId), ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(ISalesEngineEvents))]
    public class SalesEngine : ISalesEngine
    {
        #region COM
#if COM_INTEROP_ENABLED
        public const string ClassId = "97f9d82b-14de-447e-bcc8-edcc1f7bec7d";
        public const string InterfaceId = "829e57df-2f11-4708-a115-e87521dd0bab";
        public const string EventsId = "43cd3f93-a233-42d7-86ca-e8944b8b8e3b";

        // These routines perform the additional COM registration needed by ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(System.Type t)
        {
            ComRegistration.RegisterControl(t);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(System.Type t)
        {
            ComRegistration.UnregisterControl(t);
        }
#endif
        #endregion

        private bool _init;
        private List<ShoppingItem> _shoppingBag;
        private List<SalesGroup> _availableSales;
        private List<SaleDiscount> _previousSales;

        public bool Initialized { get { return _init; } }

        public event RestartHandler EngineRestarted;
        public event AppliedHandler SaleApplied;
        public event CancelHandler SaleCancelled;

        public SalesEngine()
        {
            ActivityLog.Logger.LogCall();
            EngineRestarted += () => ActivityLog.Logger.LogMessage("Engine restarted!");
            SaleApplied += sd => ActivityLog.Logger.LogMessage("Sale applied: " + sd.Title);
            SaleCancelled += id => ActivityLog.Logger.LogMessage("Sale canceled: " + id);
            _init = false;
        }

        public bool Initialize(string vipno = null)
        {
            if (Wrapper.User == null)
                return _init = false;
            ActivityLog.Logger.LogCall(vipno);
            try
            {
                _previousSales = new List<SaleDiscount>();
                _availableSales = DBService.GetService().GetAvailableSales(vipno);
                _shoppingBag = new List<ShoppingItem>();
                SaleDiscount.ResetCounter();
                if (EngineRestarted != null)
                    EngineRestarted();
                return _init = true;
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
                return _init = false;
            }
        }

        public string AddItem(string pluno, double qty, double price)
        {
            return AddItem(pluno, qty, price, null);
        }
        public string AddItem(string pluno, double qty, double price, int kind)
        {
            return AddItem(pluno, qty, price, (int?)kind);
        }
        internal string AddItem(string pluno, double qty, double price, int? kind)
        {
            ActivityLog.Logger.LogCall(pluno, qty, price, kind);
            try
            {
                if (!_init) return null;

                ShoppingItem newItem = new ShoppingItem(pluno, qty, price, kind);
                bool added = false;
                foreach (ShoppingItem si in _shoppingBag)
                    if (added = si.Add(newItem))
                        break;
                if (!added)
                    Common.InsertItemToCart(_shoppingBag, newItem);

                var sales = evaluateSales();
                Common.collapseSales(sales);
                evaluateChanges(sales);

                string title;
                for (int i = 0; i < _availableSales.Count; i++)
                    if ((title = _availableSales[i].IsProductRequire(pluno, kind)) != "")
                        return title; //### handle multiple sales' titles received
                return "";
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
                return null;
            }
        }
        public void RemoveItem(string pluno, double qty)
        {
            ActivityLog.Logger.LogCall(pluno, qty);
            try
            {
                if (!_init) return;
                var removeList = _shoppingBag.FindAll(si => si.Pluno == pluno);
                if (removeList.Count == 0)
                {
                    //### item not found
                }
                else
                {
                    if (removeList.Count > 1)
                    {
                        //### more than one instance found
                    }
                    double newAmount = removeList[0].Reduce(qty);
                    if (newAmount < 0)
                    {
                        //### remove more than exist
                        _shoppingBag.Remove(removeList[0]);
                    }
                    if (newAmount == 0)
                        _shoppingBag.Remove(removeList[0]);
                }
                var sales = evaluateSales();
                Common.collapseSales(sales);
                evaluateChanges(sales);
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
            }
        }

        public bool IsPlunoActive(string pluno)
        {
            if (!_init) return false;
            return _previousSales.Exists(sd => sd.IsContainProduct(pluno));
        }
        public SaleDiscount[] CalcAllSales()
        {
            if (!_init) return null;
            return _previousSales.ToArray();
        }

        private List<SaleDiscount> evaluateSales()
        {
            List<SaleDiscount> newSales = new List<SaleDiscount>();
            List<ShoppingItem> newBag = _shoppingBag.ConvertAll<ShoppingItem>(p => new ShoppingItem(p)); //clone
            List<SaleDiscount> efective;
            double totalReceipt = 0;
            _shoppingBag.ForEach(si => totalReceipt += si.Price * si.Amount);
            foreach (var s in _availableSales)
            {
                efective = s.GetEfective(newBag, totalReceipt);
                if (efective != null && efective.Count > 0)
                    efective.ForEach(sd => newSales.Add(sd));
            }
            return newSales;
        }
        private void evaluateChanges(List<SaleDiscount> newSales)
        {
            var previousSales = _previousSales;
            _previousSales = newSales;
            for (int i = 0; i < newSales.Count; i++)
            {
                SaleDiscount sd = newSales[i];
                var removed = previousSales.FindAll(s => s == sd);
                if (removed.Count == 0)
                {
                    sd.SetID();
                    if (SaleApplied != null)
                        SaleApplied(sd);
                }
                else if (removed.Count != 1)
                {
                    //### multiple equal sales
                }
                else // (removed.Count == 1)
                {
                    sd.SetID(removed[0].ID);
                    previousSales.RemoveAll(s => s == sd);
                }
            }
            for (int i = 0; i < previousSales.Count; i++)
            {
                SaleDiscount sd = previousSales[i];
                if (SaleCancelled != null)
                    SaleCancelled(sd.ID);
            }
        }

        //### Testing purpose
        public bool InitializeForDebugging()
        {
            ActivityLog.Logger.LogCall();
            try
            {
                _previousSales = new List<SaleDiscount>();
                _availableSales = makeSalesForTester();
                _shoppingBag = new List<ShoppingItem>();
                SaleDiscount.ResetCounter();
                if (EngineRestarted != null)
                    EngineRestarted();
                return _init = true;
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
                return _init = false;
            }
        }
        private static List<SalesGroup> makeSalesForTester()
        {
            var Sales = new List<SalesGroup>();
            Sale s;
            var reqs = new List<ProdAmount>();
            var outs = new List<DiscountedProduct>();

            // Sale 1.1 - Lowered price - 10 NIS discount on any '1_1' product 
            outs = new List<DiscountedProduct>();
            outs.Add(new DiscountedProduct("1_1", true, 1, 0,
                new Discount(10, DiscountTypes.Fix_Discount)));
            s = new Sale(SaleTypes.SingularLowerPrice,
                new SalesProperties("Sale 1.1", 0, null, 1, 1), null, outs);
            Sales.Add(new SalesGroup(s));

            // Sale 1.2 - Lowered price with a gift ('1_3' for free, 3 x '1_2' for 10 NIS)
            outs = new List<DiscountedProduct>();
            outs.Add(new DiscountedProduct("1_2", true, 3, 0,
                new Discount(10, DiscountTypes.Fix_Price),
                new GiftedProduct("1_3", true, 1, new Discount(0, DiscountTypes.Fix_Price))));
            s = new Sale(SaleTypes.SingularLowerPrice,
                new SalesProperties("Sale 1.2", 0, null, 1, 0), null, outs);
            Sales.Add(new SalesGroup(s));

            // Sale 2.1 - simple buy and get (buy 3 x '2_1' and get 2 x '2_2' for free)
            reqs = new List<ProdAmount>();
            reqs.Add(new ProdAmount("2_1", true, 3));
            outs = new List<DiscountedProduct>();
            outs.Add(new DiscountedProduct("2_2", true, 0, 2, new Discount(0, DiscountTypes.Fix_Price)));
            s = new Sale(SaleTypes.SingularBuyAndGet,
                new SalesProperties("Sale 2.1", 0, null, 0, 1), reqs, outs);
            Sales.Add(new SalesGroup(s));

            // Sale 2.2 - simple buy and get with minimum value (buy 2 x '2_3' and get 1.5 x '2_4' for free, one instance for each 200 NIS)
            reqs = new List<ProdAmount>();
            reqs.Add(new ProdAmount("2_3", true, 2));
            outs = new List<DiscountedProduct>();
            outs.Add(new DiscountedProduct("2_4", true, 0, 1.5, new Discount(0, DiscountTypes.Fix_Price)));
            s = new Sale(SaleTypes.SingularBuyAndGet,
                new SalesProperties("Sale 2.2", 200, null, 0, 1), reqs, outs);
            Sales.Add(new SalesGroup(s));

            return Sales;
        }
    }
}
