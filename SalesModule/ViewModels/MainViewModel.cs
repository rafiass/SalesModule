using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesModule.ViewModels
{
    class MainViewModel
    {

        public void OpenWindow()
        {
            var win = new Window();
            win.Content = new Views.PopupView(new SomeSaleViewModel());
            win.Show();
        }
    }
}
