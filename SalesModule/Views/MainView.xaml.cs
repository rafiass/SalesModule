using System.Windows;
using System.Windows.Controls;

namespace SalesModule.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private ViewModels.MainViewModel _vm;
        public MainView()
        {
            InitializeComponent();
            _vm = new ViewModels.MainViewModel();
            DataContext = _vm;
        }
    }
}
