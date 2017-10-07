
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class ProductFinderViewModel : PopupViewModel
    {
        private string _criteria;
        private List<IProductM> _results;

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

        public string Criteria
        {
            get { return _criteria; }
            set { if (SetProperty(ref _criteria, value)) OnPropertyChanged("SearchResults"); }
        }
        public List<IProductM> SearchResults
        { get { return _results.FindAll(p => p.Contains(Criteria)); } }

        public IProductM Choosen { get; private set; }

        public DelegateCommand<IProductM> ChooseItemCommand { get; private set; }

        public ProductFinderViewModel()
        {
            ChooseItemCommand = new DelegateCommand<IProductM>(chooseFunction);

            Choosen = null;
            _criteria = "";
            _results = DBService.GetService().GetProducts();
        }

        private void chooseFunction(IProductM choosen)
        {
            Choosen = choosen;
            CloseWindow();
        }
    }
}
