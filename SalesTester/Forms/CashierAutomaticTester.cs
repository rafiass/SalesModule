using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SalesModule;
using Test = System.Tuple<int, string, System.Func<bool>>;

namespace SalesTester
{
    internal partial class CashierAutomaticTester : Form
    {
        //NOTE: all tests refer to the tests created in SalesEngine.setSalesForTester()
        private ISalesEngine _eng;
        private List<ISaleDiscount> _engDiscounts;
        private List<TestDiscount> _cartDiscounts;
        private List<TestsGroup> _tests;
        private string _console;
        public CashierAutomaticTester()
        {
            InitializeComponent();
            ResetConsole();
            registerTests();

            _engDiscounts = new List<ISaleDiscount>();
            _cartDiscounts = new List<TestDiscount>();

            _eng = Wrapper.CreateEngine();
            _eng.EngineRestarted += () => _engDiscounts.Clear();
            _eng.SaleApplied += sd => _engDiscounts.Add(sd);
            _eng.SaleCancelled += id => _engDiscounts.RemoveAll(sd => sd.ID == id);
        }

        #region Utils
        private void registerTests()
        {
            TestsGroup g;
            _tests = new List<TestsGroup>();

            g = new TestsGroup("Lowered price sale", 1, WriteToConsole);
            g.Add(new Test(1, "Fix discount, without gift", test1_1));
            g.Add(new Test(2, "Fix Price, with a gift", test1_2));
            _tests.Add(g);

            g = new TestsGroup("Singular buy & get", 2, WriteToConsole);
            g.Add(new Test(1, "2 of X when buying 3 of Y", test2_1));
            g.Add(new Test(1, "1.5 of X when buying 2 of Y, for each 200 NIS in receipt", test2_2));
            _tests.Add(g);

            lstbx_tests.DisplayMember = "Title";
            _tests.ForEach(t => lstbx_tests.Items.Add(t));
        }

        private void WriteToConsole(string line)
        {
            if (_console != "")
                _console += Environment.NewLine;
            _console += line;
            txt_console.Text = _console;
        }
        private void ResetConsole()
        {
            _console = "";
            txt_console.Text = _console;
        }

        private void AddDiscount(string salesName, double qty, double discount)
        {
            var td = new TestDiscount(salesName, qty, discount);
            for (int i = 0; i < _cartDiscounts.Count; i++)
                if (_cartDiscounts[i].Title == td.Title &&
                    _cartDiscounts[i].Discount == td.Discount)
                {
                    _cartDiscounts[i].Quantity += td.Quantity;
                    return;
                }
            _cartDiscounts.Add(td);
        }
        private void RemoveDiscount(string salesName, double qty, double discount)
        {
            var td = new TestDiscount(salesName, qty, discount);
            var d = _cartDiscounts.Find(t => t.Title == td.Title && t.Discount == td.Discount);
            d.Quantity -= td.Quantity;
        }

        private bool CompareCarts(string msg)
        {
            bool discountFound = true;
            var cart = new List<TestDiscount>();
            _cartDiscounts.ForEach(d => cart.Add(d));
            for (int i = 0; i < _engDiscounts.Count; i++)
            {
                discountFound = false;
                for (int j = 0; j < cart.Count; j++)
                {
                    if (_engDiscounts[i].Discount == cart[j].Discount &&
                        _engDiscounts[i].Quantity == cart[j].Quantity &&
                        _engDiscounts[i].Title == cart[j].Title)
                    {
                        discountFound = true;
                        cart.RemoveAt(j);
                        break;
                    }
                }
                if (!discountFound)
                    break;
            }
            var isEqual = cart.Count == 0 && discountFound;
            if (!isEqual)
                WriteToConsole("Error: " + msg);
            return isEqual;
        }

        private void InitTest()
        {
            _cartDiscounts.Clear();
            _eng.InitializeForDebugging();
        }
        #endregion

        #region Tests
        /*
         * - tests for lowered price discount
         * - tests for buy and get (simple)
         * - tests for buy and get (advanced)
         * - tests for recurrences
         * - tests for multiple instances (w/o minimum != 0)
         * - tests for remove item from cart
         */

        public bool test1_1()
        {
            // Sale 1.1 - Lowered price - 10 NIS discount on any '1_1' product 
            string saleName = "Sale 1.1";
            InitTest();

            AddDiscount(saleName, 1, 10);
            _eng.AddItem("1_1", 1, 100);
            if (!CompareCarts("first insert")) return false;

            AddDiscount(saleName, 2, 10);
            _eng.AddItem("1_1", 2, 100);
            if (!CompareCarts("second insert")) return false;

            //not effecting
            _eng.AddItem("1_0", 1000, 100);
            if (!CompareCarts("insert unrelated")) return false;

            AddDiscount(saleName, 10, 10);
            _eng.AddItem("1_1", 10, 100);
            if (!CompareCarts("third insert")) return false;

            RemoveDiscount(saleName, 3, 10);
            _eng.RemoveItem("1_1", 3);
            if (!CompareCarts("removing item")) return false;

            _eng.RemoveItem("1_0", 10);
            if (!CompareCarts("removing unrelated")) return false;

            return true;
        }
        public bool test1_2()
        {
            // Sale 1.2 - Lowered price with a gift ('1_3' for free, 3 x '1_2' for 10 NIS)
            string saleName = "Sale 1.2";
            InitTest();

            AddDiscount(saleName, 1, 35);
            _eng.AddItem("1_2", 3, 15);
            if (!CompareCarts("first package")) return false;

            AddDiscount(saleName, 1, 20);
            _eng.AddItem("1_3", 1, 20);
            if (!CompareCarts("first gift")) return false;

            _eng.AddItem("1_2", 2, 15);
            if (!CompareCarts("second package")) return false;
            _eng.AddItem("1_3", 2, 30); // only one of those will include
            if (!CompareCarts("second gift")) return false;
            AddDiscount(saleName, 1, 30);
            AddDiscount(saleName, 1, 40);
            _eng.AddItem("1_2", 1, 20);//different prices
            if (!CompareCarts("second package")) return false;

            AddDiscount(saleName, 1, 30);
            AddDiscount(saleName, 1, 110);
            _eng.AddItem("1_2", 3, 40);
            if (!CompareCarts("third package")) return false;

            //removing multi-price product is not supported yet
            //RemoveDiscount(saleName, 1, 30);
            //RemoveDiscount(saleName, 1, 35);
            //_eng.RemoveItem("1_2", 1); //removing the cheapest one
            //if (!CompareCarts("removing third package")) return false;

            return true;
        }

        public bool test2_1()
        {
            // Sale 2.1 - simple buy and get (buy 3 x '2_1' and get 2 x '2_2' for free)
            string saleName = "Sale 2.1";
            InitTest();

            _eng.AddItem("2_1", 2, 100);
            if (!CompareCarts("first products insert")) return false;
            _eng.AddItem("2_2", 3, 50);
            if (!CompareCarts("first gifts insert")) return false;
            AddDiscount(saleName, 2, 50);
            _eng.AddItem("2_1", 2, 75);
            if (!CompareCarts("first sale")) return false;

            //there is a '2_1' and a '2_2' that is out of sales, let's use them

            AddDiscount(saleName, 1, 50);
            _eng.AddItem("2_1", 2, 110);
            if (!CompareCarts("leftovers products sale")) return false;
            AddDiscount(saleName, 1, 75);
            _eng.AddItem("2_2", 2, 75);
            if (!CompareCarts("leftovers products sale")) return false;

            return true;
        }
        public bool test2_2()
        {
            // Sale 2.2 - simple buy and get with minimum value (buy 2 x '2_3' and get 1.5 x '2_4' for free, one instance for each 200 NIS)
            string saleName = "Sale 2.2";
            InitTest();

            _eng.AddItem("2_3", 2, 17.5);
            if (!CompareCarts("first insert")) return false;
            _eng.AddItem("2_4", 1.5, 10);
            if (!CompareCarts("first insert")) return false;
            _eng.AddItem("2_0", 149, 1);
            if (!CompareCarts("unrelated under the minimum")) return false;
            AddDiscount(saleName, 1.5, 10);
            _eng.AddItem("2_0", 1, 1);
            if (!CompareCarts("first sale")) return false;

            _eng.AddItem("2_0", 250, 1);
            if (!CompareCarts("unrelated products")) return false;
            _eng.AddItem("2_3", 3, 17.5);
            if (!CompareCarts("second insert")) return false;
            AddDiscount(saleName, 1.5, 15);
            _eng.AddItem("2_4", 2.5, 15);
            if (!CompareCarts("second sale")) return false;

            //unused for now: 1x2_4, 1x2_3, 125 NIS

            _eng.AddItem("2_3", 2, 20);
            if (!CompareCarts("third insert")) return false;
            AddDiscount(saleName, 1, 15);
            _eng.AddItem("2_0", 1, 35);
            if (!CompareCarts("third sale")) return false;

            return true;
        }
        #endregion

        #region GUI
        private void btn_run_Click(object sender, EventArgs e)
        {
            var test = lstbx_tests.SelectedItem as TestsGroup;
            if (test == null)
            {
                btn_run.Enabled = false;
                return;
            }
            test.Execute();
        }
        private void btn_all_Click(object sender, EventArgs e)
        {
            foreach (var test in _tests)
                test.Execute();
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            ResetConsole();
        }

        private void lstbx_tests_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_run.Enabled = lstbx_tests.SelectedIndex != -1;
        }
        private void lstbx_tests_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lstbx_tests.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var test = lstbx_tests.Items[index] as TestsGroup;
                test.Execute();
            }
        }
        #endregion
    }

    internal class TestDiscount
    {
        public string Title { get; private set; }
        public double Quantity { get; set; }
        public double Discount { get; private set; }

        public TestDiscount(string title, double qty, double disc)
        {
            Title = title;
            Quantity = qty;
            Discount = disc;
        }
    }

    internal class TestsGroup
    {
        private Action<string> _writeToConsole;

        public string Title { get; private set; }
        public int ID { get; private set; }
        public List<Test> Tests { get; private set; }

        public TestsGroup(string title, int id, Action<string> consoleWriter)
        {
            Title = title;
            ID = id;
            Tests = new List<Test>();
            _writeToConsole = consoleWriter;
        }

        public void Add(Test test)
        {
            Tests.Add(test);
        }

        public void Execute()
        {
            _writeToConsole("       Testing - '" + Title + "':");
            _writeToConsole("");
            for (int i = 0; i < Tests.Count; i++)
            {
                _writeToConsole("#" + ID + "." + Tests[i].Item1 + ") Testing " + Tests[i].Item2);
                if (Tests[i].Item3())
                    _writeToConsole("Test finished successfully!");
                else
                    _writeToConsole("Test failed!");
            }
            _writeToConsole("");
        }
    }
}
