using System.Windows;
using SalesModule.ViewModels;
using SalesModule.Views;

namespace SalesModule.Services
{
    static class InteropService
    {
        public static void OpenWindow(ViewModelBase vm, PopupProperties prop = null)
        {
            var win = new Window();
            //### window properties
            win.Content = new PopupView(vm);
            win.Show();
        }
    }
}
