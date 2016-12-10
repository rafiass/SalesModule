using System.Data;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class TestViewModel : PopupViewModel
    {
        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "חלון חיפוש",
                    Width = 900,
                    Height = 500
                };
            }
        }
        
        public DataTable DataTable { get; private set; }

        public TestViewModel()
        {
            DataTable = DBService.GetService().Test();
        }
    }
}
