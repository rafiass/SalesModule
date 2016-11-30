using System.Windows.Controls;

namespace SalesModule.Views
{
    /// <summary>
    /// Interaction logic for PopupView.xaml
    /// </summary>
    public partial class PopupView : UserControl
    {
        public PopupView(object vm)
        {
            InitializeComponent();
            presenter.Content = vm;
        }
    }
}
