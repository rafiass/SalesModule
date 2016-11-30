using System;
using System.Windows.Forms;
using SalesModule;

namespace SalesTester
{
    public partial class MainWindow : Form
    {
        private bool isInit;
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool init()
        {
            if (!isInit)
            {
                var w = new Wrapper();
                InitResults res = w.Init("40.113.20.172,44455", "Sales", "Sales", "R0h3niu123", 1, "admin", "1234");
                isInit = w.IsInited();
                if (isInit && res == InitResults.Success)//isInit = w.IsInited())
                    Text = "Testing version: " + w.Version;
            }
            return isInit;
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if (init())
                (new CreateSale()).ShowDialog();
            else
                MessageBox.Show("אין אפשרות להציג את המודל.");
        }
        private void btn_manual_Click(object sender, EventArgs e)
        {
            if (init())
                (new CashierManualTester()).ShowDialog();
            else
                MessageBox.Show("אין אפשרות להציג את המודל.");
        }
        private void btn_auto_Click(object sender, EventArgs e)
        {
            if (init())
                (new CashierAutomaticTester()).ShowDialog();
            else
                MessageBox.Show("אין אפשרות להציג את המודל.");
        }
    }
}
