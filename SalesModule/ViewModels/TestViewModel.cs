using System.Collections.ObjectModel;
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
                    Title = "טסט",
                    Width = 500,
                    Height = 400,
                    IsModal = true
                };
            }
        }

        private SalesEngine _engine;
        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public TestViewModel()
        {
            _engine = new SalesEngine();
            _engine.Initialize();
            _engine.SaleApplied += s => Status = s.Title + " Applied!";
            _engine.SaleCancelled += id => Status = id + " Cancelled!";
            _engine.EngineRestarted += () => Status = "restarted";
        }
    }
}
