using SalesModule.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SalesModule.Services
{
    internal static class InteropService
    {
        public static void OpenWindow<T>(Action callbackFunction = null)
            where T : PopupViewModel, new()
        {
            OpenWindow(new T(), callbackFunction);
        }
        public static void OpenWindow(PopupViewModel vm, Action callbackFunction = null)
        {
            OpenWindow(vm, vm.PopupProperties, callbackFunction);
        }
        public static void OpenWindow(object vm, PopupProperties prop, Action callbackFunction = null)
        {
            if (vm == null) return;
            ActivityLogService.Logger.LogFunctionCall(vm);
            prop = prop ?? new PopupProperties();
            callbackFunction = callbackFunction ?? (() => { });
            var winExtraWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
            var winExtraHeight = SystemParameters.ResizeFrameHorizontalBorderHeight + SystemParameters.CaptionHeight;
            var win = new Window()
            {
                Title = prop.Title,
                Width = prop.Width + winExtraWidth,
                Height = prop.Height + winExtraHeight,
                MinWidth = prop.MinWidth + winExtraWidth,
                MinHeight = prop.MinHeight + winExtraHeight,
                ShowInTaskbar = prop.IsShowingOnTaskBar,
                Content = vm,
                FlowDirection = FlowDirection.RightToLeft,
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240))
            };
            win.Resources.Source = new Uri(
                "pack://application:,,,/SalesModule;component/Resources/ResourceDictionary.xaml", UriKind.Absolute);
            win.InputBindings.Add(new KeyBinding(new DelegateCommand(win.Close), new KeyGesture(Key.Escape)));
            win.Closed += (s, e) => callbackFunction();

            if (vm is PopupViewModel pvm)
            {
                win.Closed += (s, e) => pvm.WindowClosed();
                win.Closing += (s, e) => pvm.WindowClosing(e);
                pvm.SetCloseAction(win.Close);
            }

            if (prop.IsModal)
                win.ShowDialog();
            else
                win.Show();
        }

        public static double? GetNumber(string title, double? num = null)
        {
            ActivityLogService.Logger.LogFunctionCall(title, num);
            var vm = num == null ? new NumPadViewModel(title) : new NumPadViewModel(title, (double)num);
            OpenWindow(vm);
            return vm.Results;
        }
    }
}
