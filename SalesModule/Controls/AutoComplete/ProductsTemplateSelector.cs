using System.IO;
using System.Windows;
using System.Windows.Controls;
using SalesModule.Models;

namespace SalesModule.Controls
{
    internal class ProductsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ProductTemplate { get; set; }
        public DataTemplate CategoryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ProductM)
                return ProductTemplate;
            else if (item is CategoryM)
                return CategoryTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
