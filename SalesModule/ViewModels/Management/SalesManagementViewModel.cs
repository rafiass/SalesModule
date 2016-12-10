﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal class SaleGroupViewModel : ViewModelBase
    {
        private bool _isEnabled;

        public int GroupID { get; private set; }
        public string Title { get; private set; }
        public string Ename { get; private set; }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (SetProperty(ref _isEnabled, value))
                    DBService.GetService().DisableSaleGroupM(GroupID, value);
            }
        }
        public DateTime DateCreated { get; private set; }

        public SaleGroupViewModel(int groupID, string title, string ename, bool isEnabled, DateTime creationDate)
        {
            GroupID = groupID;
            Title = title;
            Ename = ename;
            _isEnabled = isEnabled;
            DateCreated = creationDate;
        }

        public override string ToString()
        {
            return "GroupID = " + GroupID + ", Title = " + Title + ", Ename = " + Ename +
                   ", IsEnabled = " + IsEnabled + ", DateCreated = " + DateCreated;
        }
    }

    internal class SalesManagementViewModel : PopupViewModel
    {
        private string _criteria;
        private List<SaleGroupViewModel> _groups;

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "חלון ניהול",
                    Width = 900,
                    MinWidth = 650,
                    Height = 500
                };
            }
        }

        public string Criteria
        {
            get { return _criteria; }
            set { if (SetProperty(ref _criteria, value)) OnPropertyChanged("Groups"); }
        }

        public List<SaleGroupViewModel> Groups
        { get { return _groups.FindAll(g => g.Ename.Contains(Criteria) || g.Title.Contains(Criteria)); } }

        public DelegateCommand<SaleGroupViewModel> EditSaleCommand { get; private set; }
        public DelegateCommand<SaleGroupViewModel> GroupPCIDCommand { get; private set; }
        public DelegateCommand<SaleGroupViewModel> GroupVipCommand { get; private set; }

        public SalesManagementViewModel()
        {
            EditSaleCommand = new DelegateCommand<SaleGroupViewModel>(editFunction);
            GroupPCIDCommand = new DelegateCommand<SaleGroupViewModel>(pcidFunction);
            GroupVipCommand = new DelegateCommand<SaleGroupViewModel>(vipFunction);

            _criteria = "";
            refreshGroups();
        }

        private void refreshGroups()
        {
            var s = DBService.GetService().GetAllSalesTitles();
            _groups = new List<SaleGroupViewModel>();
            foreach (DataRow r in s.Rows)
                _groups.Add(new SaleGroupViewModel(
                    int.Parse(r["GroupID"].ToString()), r["Title"].ToString(), r["ename"].ToString(),
                    bool.Parse(r["isEnabled"].ToString()), DateTime.Parse(r["DateCreated"].ToString())));
            OnPropertyChanged("Groups");
        }

        public void editFunction(SaleGroupViewModel sgvm)
        {
            ActivityLogService.Logger.LogCall(sgvm.GroupID);
            try
            {
                var group = DBService.GetService().LoadGroup(sgvm.GroupID);
                if (group.Sales.Count == 1)
                {
                    var sale = group.Sales[0];
                    sale = SalesFactoryService.EditSale(sale);
                    if (sale == null) return;

                    if (DBService.GetService().EditSaleM(sale))
                        MessageBox.Show("המבצע עודכן בהצלחה!");
                    else
                        MessageBox.Show("אירעה שגיאה בזמן עריכת המבצע.\nלא בוצעו שינויים במבצע הקיים.");
                }
                else
                {
                    //TODO: edit group form
                    MessageBox.Show("כרגע, לא ניתן לערוך קבוצת מבצעים.");
                }
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
            }
        }
        public void pcidFunction(SaleGroupViewModel sgvm)
        {
            ActivityLogService.Logger.LogCall(sgvm.GroupID);
            var pcidVM = new PCIDAssociationViewModel(sgvm.GroupID);
            InteropService.OpenWindow(pcidVM, pcidVM.PopupProperties);
        }
        public void vipFunction(SaleGroupViewModel sgvm)
        {
            ActivityLogService.Logger.LogCall(sgvm.GroupID);
            //var vipVM = new VipAssociationViewModel(sgvm.GroupID);
            //InteropService.OpenWindow(vipVM, null);
        }
    }
}