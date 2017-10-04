using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SalesModule.Models;
using SalesModule.Services;

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
        bool LoadSales();

        string AddItem(string pluno, double qty, double price, bool evalSales = true);
        string AddItem(string pluno, double qty, double price, int kind, bool evalSales = true);
        void RemoveItem(string pluno, double qty);

        void EvaluateSales();

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

        private object _locker = new object();
        private bool _init;
        private string _vipno;
        private List<SalesGroupM> _availableSales;
        private List<ShoppingItem> _shoppingBag;
        private List<SaleDiscount> _previousSales;
        private List<SalesGroupM> AvailableSales
        {
            get
            {
                if (_availableSales == null)
                    _availableSales = DBService.GetService().GetAvailableSales(_vipno);
                return _availableSales;
            }
            set { _availableSales = value; }
        }

        public bool Initialized { get { return _init; } }

        public event RestartHandler EngineRestarted;
        public event AppliedHandler SaleApplied;
        public event CancelHandler SaleCancelled;

        public SalesEngine()
        {
            ActivityLogService.Logger.LogCall();
            EngineRestarted += () => ActivityLogService.Logger.LogMessage("Engine restarted!");
            SaleApplied += sd => ActivityLogService.Logger.LogMessage("Sale applied: " + sd.Title + "(" + sd.SaleID + "), Id = " + sd.ID);
            SaleCancelled += id => ActivityLogService.Logger.LogMessage("Sale canceled: " + id);
            _init = false;
        }

        public bool Initialize(string vipno = null)
        {
            lock (_locker)
            {
                if (Wrapper.User == null)
                    return _init = false;
                ActivityLogService.Logger.LogCall();
                try
                {
                    _vipno = vipno;
                    _previousSales = new List<SaleDiscount>();
                    _shoppingBag = new List<ShoppingItem>();
                    SaleDiscount.ResetCounter();
                    if (EngineRestarted != null)
                        EngineRestarted();
                    return _init = true;
                }
                catch (Exception ex)
                {
                    ActivityLogService.Logger.LogError(ex);
                    return _init = false;
                }
            }
        }
        public bool LoadSales()
        {
            lock (_locker)
            {
                if (Wrapper.User == null)
                    return _init = false;
                ActivityLogService.Logger.LogCall();
                try
                {
                    AvailableSales = DBService.GetService().GetAvailableSales(_vipno);
                    return AvailableSales != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string AddItem(string pluno, double qty, double price, bool evalSales = true)
        {
            lock (_locker)
                return AddItem(pluno, qty, price, null, evalSales);
        }
        public string AddItem(string pluno, double qty, double price, int kind, bool evalSales = true)
        {
            lock (_locker)
                return AddItem(pluno, qty, price, (int?)kind, evalSales);
        }
        internal string AddItem(string pluno, double qty, double price, int? kind, bool evalSales)
        {
            ActivityLogService.Logger.LogCall(pluno, qty, price, kind, evalSales);
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

                if (!evalSales) return "";

                var sales = evaluateSales();
                Common.collapseSales(sales);
                evaluateChanges(sales);

                string title;
                for (int i = 0; i < AvailableSales.Count; i++)
                    if ((title = AvailableSales[i].IsProductRequire(pluno, kind)) != "")
                        return title; //### handle multiple sales' titles received
                return "";
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
        }
        public void RemoveItem(string pluno, double qty)
        {
            lock (_locker)
            {
                ActivityLogService.Logger.LogCall(pluno, qty);
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
                    ActivityLogService.Logger.LogError(ex);
                }
            }
        }

        public void EvaluateSales()
        {
            ActivityLogService.Logger.LogCall();
            try
            {
                if (!_init) return;

                var sales = evaluateSales();
                Common.collapseSales(sales);
                evaluateChanges(sales);
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
            }
        }

        public bool IsPlunoActive(string pluno)
        {
            if (!_init) return false;
            lock (_locker)
                return _previousSales.Exists(sd => sd.IsContainProduct(pluno));
        }
        public SaleDiscount[] CalcAllSales()
        {
            if (!_init) return null;
            lock (_locker)
                return _previousSales.ToArray();
        }

        private List<SaleDiscount> evaluateSales()
        {
            List<SaleDiscount> newSales = new List<SaleDiscount>();
            List<ShoppingItem> newBag = _shoppingBag.ConvertAll(p => new ShoppingItem(p)); //clone
            List<SaleDiscount> efective;
            double totalReceipt = 0;
            _shoppingBag.ForEach(si => totalReceipt += si.Price * si.Amount);
            foreach (var s in AvailableSales)
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
            lock (_locker)
            {
                ActivityLogService.Logger.LogCall();
                try
                {
                    _previousSales = new List<SaleDiscount>();
                    AvailableSales = makeSalesForTester();
                    _shoppingBag = new List<ShoppingItem>();
                    SaleDiscount.ResetCounter();
                    if (EngineRestarted != null)
                        EngineRestarted();
                    return _init = true;
                }
                catch (Exception ex)
                {
                    ActivityLogService.Logger.LogError(ex);
                    return _init = false;
                }
            }
        }
        private static List<SalesGroupM> makeSalesForTester()
        {
            var Sales = new List<SalesGroupM>();
            SaleM s;
            var reqs = new List<ProdAmountM>();
            var outs = new List<DiscountedProductM>();

            // Sale 1.1 - Lowered price - 10 NIS discount on any '1_1' product 
            outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM("1_1", true, 1, 0,
                new DiscountM(10, DiscountTypes.Fix_Discount)));
            s = new SaleM(SaleTypes.SingularLowerPrice,
                new SalesPropertiesM("Sale 1.1", 0, null, 1, 1), null, outs);
            Sales.Add(new SalesGroupM(s));

            // Sale 1.2 - Lowered price with a gift ('1_3' for free, 3 x '1_2' for 10 NIS)
            outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM("1_2", true, 3, 0,
                new DiscountM(10, DiscountTypes.Fix_Price),
                new GiftedProductM("1_3", true, 1, new DiscountM(0, DiscountTypes.Fix_Price))));
            s = new SaleM(SaleTypes.SingularLowerPrice,
                new SalesPropertiesM("Sale 1.2", 0, null, 1, 0), null, outs);
            Sales.Add(new SalesGroupM(s));

            // Sale 2.1 - simple buy and get (buy 3 x '2_1' and get 2 x '2_2' for free)
            reqs = new List<ProdAmountM>();
            reqs.Add(new ProdAmountM("2_1", true, 3));
            outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM("2_2", true, 0, 2, new DiscountM(0, DiscountTypes.Fix_Price)));
            s = new SaleM(SaleTypes.SingularBuyAndGet,
                new SalesPropertiesM("Sale 2.1", 0, null, 0, 1), reqs, outs);
            Sales.Add(new SalesGroupM(s));

            // Sale 2.2 - simple buy and get with minimum value (buy 2 x '2_3' and get 1.5 x '2_4' for free, one instance for each 200 NIS)
            reqs = new List<ProdAmountM>();
            reqs.Add(new ProdAmountM("2_3", true, 2));
            outs = new List<DiscountedProductM>();
            outs.Add(new DiscountedProductM("2_4", true, 0, 1.5, new DiscountM(0, DiscountTypes.Fix_Price)));
            s = new SaleM(SaleTypes.SingularBuyAndGet,
                new SalesPropertiesM("Sale 2.2", 200, null, 0, 1), reqs, outs);
            Sales.Add(new SalesGroupM(s));

            return Sales;
        }
    }
}
