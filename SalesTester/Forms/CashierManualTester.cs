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
        private int _ActionID;
        private SalesEngine Engine;
        private List<SaleDiscount> _sales;
        public CashierManualTester()
        {
            InitializeComponent();
            _ActionID = 0;
            _sales = new List<SaleDiscount>();
            InitEngine();
        }
        private int getID()
        {
            return ++_ActionID;
        }
        private void InitEngine()
        {
            Engine = new SalesEngine();
            Engine.InitializeForDebugging();
            Engine.EngineRestarted += CallReset;
            Engine.SaleApplied += CallApplied;
            Engine.SaleCancelled += CallRemoved;
        }

        private void addProduct(string pluno, double qty, double price)
        {
            string str = getID() + ". Product " + pluno + ": " + qty;
            listCashier.Items.Add(new KeyValuePair<int, string>(-1, str));
            lblInSale.Text = Engine.AddItem(pluno, qty, price);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addProduct("1", 2, 100);
            addProduct("2", 1, 200);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            addProduct("3", 2, 105);
            addProduct("4", 2, 200);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            addProduct("999", 1, 2000);
            addProduct("5", 1, 200);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Do nothing.");
        }
        private void btnDebug_Click(object sender, EventArgs e)
        {
            Engine.InitializeForDebugging();
        }

        private void CallReset()
        {
            _sales = new List<SaleDiscount>();
            listCashier.Items.Clear();
            listActions.Items.Clear();
            MessageBox.Show(getID() + ". Module Reseted");
            _ActionID = 0;
        }
        private void CallApplied(SaleDiscount sd)
        {
            _sales.Add(sd);
            string sdStr = getID() + ". " + sd.ID + ": " + sd.Title + ", " + sd.Discount + " * " + sd.Quantity;
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
