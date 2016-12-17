using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
                Title = prop.Title,
                Width = prop.Width,
                Height = prop.Height,
                MinWidth = prop.MinWidth,
                MinHeight = prop.MinHeigth,
                ShowInTaskbar = prop.IsShowingOnTaskBar,
                Content = vm,
                FlowDirection = FlowDirection.RightToLeft,
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240))
            };
            win.Resources.Source = new Uri(
                "pack://application:,,,/SalesModule;component/Resources/ResourceDictionary.xaml", UriKind.Absolute);
            win.InputBindings.Add(new KeyBinding(new DelegateCommand(win.Close), new KeyGesture(Key.Escape)));
            win.Closed += (s, e) => callbackFunction();

            if (vm is PopupViewModel)
            {
                win.Closed += (s, e) => (vm as PopupViewModel).WindowClosed();
                win.Closing += (s, e) => (vm as PopupViewModel).WindowClosing(e);
                (vm as PopupViewModel).SetCloseAction(win.Close);
            }

            if (prop.IsModal)
                win.ShowDialog();
            else
                win.Show();
        }

        public static double? GetNumber(string title, double? num = null)
        {
            ActivityLogService.Logger.LogCall(title, num);
            var vm = num == null ? new NumPadViewModel(title) : new NumPadViewModel(title, (double)num);
            OpenWindow(vm, vm.PopupProperties);
            return vm.Results;
        }
    }
}
