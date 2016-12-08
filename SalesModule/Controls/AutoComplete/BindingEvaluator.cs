using System.Windows;
using System.Windows.Data;

namespace SalesModule.Controls
{
    internal class BindingEvaluator : FrameworkElement
    {
        private Binding _valueBinding;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(BindingEvaluator), new FrameworkPropertyMetadata(string.Empty));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public Binding ValueBinding
        {
            get { return _valueBinding; }
            set { _valueBinding = value; }
        }

        public BindingEvaluator(Binding binding)
        {
            ValueBinding = binding;
        }

        public string Evaluate(object dataItem)
        {
            this.DataContext = dataItem;
            SetBinding(ValueProperty, ValueBinding);
            return Value;
        }
    }
}
