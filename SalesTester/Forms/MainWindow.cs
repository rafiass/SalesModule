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
                InitResults res = w.Init("newordersqlmain.database.windows.net", "SALES_TEST", "yoni", "Yy123456789", 1, "admin", "1234");
                isInit = w.IsInited();
                if (isInit && res == InitResults.Success)
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
