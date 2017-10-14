using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesModule;

namespace SalesTester
{
    public partial class CashierManualTester : Form
    {
        private int _actionID;
        private ISalesEngine _engine;
        private List<ISaleDiscount> _sales;
        public CashierManualTester()
        {
            InitializeComponent();
            _actionID = 0;
            _sales = new List<ISaleDiscount>();
            InitEngine();
            _engine.LoadSales();
        }
        private int getID()
        {
            return ++_actionID;
        }
        private void InitEngine()
        {
            _engine = Wrapper.CreateEngine();
            _engine.Initialize();
            //Engine.InitializeForDebugging();
            _engine.EngineRestarted += CallReset;
            _engine.SaleApplied += CallApplied;
            _engine.SaleCancelled += CallRemoved;
        }

        private void addProduct(string pluno, double qty, double price)
        {
            string str = getID() + ". Product " + pluno + ": " + qty + " * " + price;
            listCashier.Items.Add(new KeyValuePair<int, string>(-1, str));
            lblInSale.Text = _engine.AddItem(pluno, qty, price, checkBox1.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addProduct("999902", 1, 5);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            addProduct("10006", 1, 10);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            addProduct("10003", 1, 10);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            addProduct("10004", 1, 9);
        }
        private void btnDebug_Click(object sender, EventArgs e)
        {
            _engine.Initialize();
            //Engine.InitializeForDebugging();
        }

        private void CallReset()
        {
            _sales = new List<ISaleDiscount>();
            listCashier.Items.Clear();
            listActions.Items.Clear();
            MessageBox.Show(getID() + ". Module Reseted");
            _actionID = 0;
        }
        private void CallApplied(ISaleDiscount sd)
        {
            _sales.Add(sd);
            string sdStr = getID() + ". " + sd.ID + ": " + sd.Title + ", Discount = " + sd.Discount + " * " + sd.Quantity;
            listCashier.Items.Add(new KeyValuePair<int, string>(sd.ID, sdStr));
            listActions.Items.Add(getID() + ". " + sd.ID + " Added.");
        }
        private void CallRemoved(int id)
        {
            for (int i = 0; i < listCashier.Items.Count; i++)
                if (((KeyValuePair<int, string>)listCashier.Items[i]).Key == id)
                {
                    listCashier.Items.RemoveAt(i);
                    break;
                }
            listActions.Items.Add(getID() + ". " + id + " Removed.");
        }
    }
}
