using System;
using System.Windows.Forms;

namespace SalesModule.GUI
{
    public partial class TestForm : Form
    {
        private SalesEngine _engine;
        public TestForm()
        {
            InitializeComponent();
            _engine = new SalesEngine();
            _engine.Initialize();
            _engine.LoadSales();
            _engine.SaleApplied += s => label2.Text = s.Title + " Applied!";
            _engine.SaleCancelled += id => label2.Text = id + " Cancelled!";
            _engine.EngineRestarted += () => label2.Text = "restarted";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _engine.AddItem("1003", 1, 10, 1);
            _engine.AddItem("1001", 1, 10, 1);
            _engine.AddItem("1004", 1, 9, 1);
            _engine.AddItem("1002", 1, 10, 1);
        }
    }
}
