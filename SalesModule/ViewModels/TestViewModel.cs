﻿using System.Data;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class TestViewModel : PopupViewModel
    {
        private string _criteria;
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
            set { SetProperty(ref _criteria, value); }
        }
        public DataTable DataTable { get { return DBService.GetService().Test(Criteria); } }

        public DelegateCommand RefreshCommand { get; private set; }

        public TestViewModel()
        {
            RefreshCommand = new DelegateCommand(() => OnPropertyChanged("DataTable"));
        }
    }
}
