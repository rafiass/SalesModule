using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SalesModule.Services;

namespace SalesModule.Controls
{
    internal class TouchableNumeric : Control
    {
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment", typeof(double), typeof(TouchableNumeric), new PropertyMetadata(1.0));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(TouchableNumeric), new PropertyMetadata(0.0, callback));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double?), typeof(TouchableNumeric), new PropertyMetadata(null, callback));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(TouchableNumeric), new PropertyMetadata(0.0, callback));

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(
            "DecimalPlaces", typeof(int), typeof(TouchableNumeric), new PropertyMetadata(3, callback));

        public static readonly DependencyProperty UpCommandProperty = DependencyProperty.Register(
            "UpCommand", typeof(ICommand), typeof(TouchableNumeric), new PropertyMetadata(null));
        public static readonly DependencyProperty DownCommandProperty = DependencyProperty.Register(
            "DownCommand", typeof(ICommand), typeof(TouchableNumeric), new PropertyMetadata(null));
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand", typeof(ICommand), typeof(TouchableNumeric), new PropertyMetadata(null));

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public double? Maximum
        {
            get { return (double?)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        public ICommand UpCommand
        {
            get { return (ICommand)GetValue(UpCommandProperty); }
            private set { SetValue(UpCommandProperty, value); }
        }
        public ICommand DownCommand
        {
            get { return (ICommand)GetValue(DownCommandProperty); }
            private set { SetValue(DownCommandProperty, value); }
        }
        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            private set { SetValue(ClickCommandProperty, value); }
        }

        public TouchableNumeric()
        {
            UpCommand = new DelegateCommand(() => Value += Increment);
            DownCommand = new DelegateCommand(() => Value -= Increment);
            ClickCommand = new DelegateCommand(clickFunction);
        }

        private static void callback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TouchableNumeric)
                ((TouchableNumeric)sender).callback();
        }
        private void callback()
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Maximum != null && Value > (double)Maximum)
                Value = (double)Maximum;
            var pow = Math.Pow(10, DecimalPlaces);
            Value = Math.Floor(Value * pow) / pow;
        }

        private void clickFunction()
        {
            Value = InteropService.GetNumber(Value);
        }
    }
}
