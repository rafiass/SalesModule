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
                Wrapper w;
                //isInit = (new Wrapper()).Init("Demo", "אדמין", "1234", 1);//demo password: bR0h3niu123demo
                //isInit = (new Wrapper()).Init("server.neworder.co.il", "Demo", "demo", "bR0h3niu123demo", 1, "אדמין", "1234");
                InitResults res = (w = new Wrapper()).Init("server.neworder.co.il", "Demo", "neworder", "R0h3niu123", 1, "admin", "1234");
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
            (new CashierAutomaticTester()).ShowDialog();
        }
    }
}
