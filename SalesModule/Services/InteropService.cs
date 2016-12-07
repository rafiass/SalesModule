using System;
using System.Windows;
using System.Windows.Controls;
using SalesModule.ViewModels;
using SalesModule.Views;

namespace SalesModule.Services
{
    internal static class InteropService
    {
        public static void OpenWindow(object vm, PopupProperties prop = null, Action callbackFunction = null)
        {
            if (vm == null) return;
            ActivityLogService.Logger.LogCall(vm);
            prop = prop ?? new PopupProperties();
            callbackFunction = callbackFunction ?? (() => { });
            var win = new Window()
            {
                Width = prop.Width,
                Height = prop.Height,
                ShowInTaskbar = prop.IsShowingOnTaskBar,
                Content = vm
            };
            win.Resources.Source = new Uri(
                "pack://application:,,,/SalesModule;component/Resources/ResourceDictionary.xaml", UriKind.Absolute);
            win.Closed += (s, e) => callbackFunction();
            if (vm is PopupViewModel)
            {
                win.Closed += (s, e) => (vm as PopupViewModel).WindowClosed();
                (vm as PopupViewModel).SetCloseAction(win.Close);
            }

            if (prop.IsModal)
                win.ShowDialog();
            else
                win.Show();
        }

        public static double GetNumber(double? num = null)
        {
            ActivityLogService.Logger.LogCall(num);
            var vm = num == null ? new NumPadViewModel() : new NumPadViewModel((double)num);
            var prop = new PopupProperties()
            {
                //### GetNumber PopupProperties
            };
            OpenWindow(vm, prop);
            return vm.Value;
        }
    }
}
