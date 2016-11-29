using System;
using System.Collections.Generic;
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
            _engine.SaleApplied += s => label2.Text = s.Title + " Applied!";
            _engine.SaleCancelled += id => label2.Text = id + " Cancelled!";
            _engine.EngineRestarted += () => label2.Text = "restarted";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = _engine.AddItem("10124", 1, 39);
        }
    }
}
