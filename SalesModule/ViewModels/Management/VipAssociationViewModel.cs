using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using SalesModule.Services;

namespace SalesModule.ViewModels
{
    internal interface IVipViewModel
    {
        int ID { get; }
        string Name { get; }
        string Type { get; }

        bool Contains(string filter);
    }

    internal class PrivateVipViewModel : IVipViewModel
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get { return "פרטי"; } }

        public PrivateVipViewModel(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public bool Contains(string filter)
        {
            return ID.ToString().Contains(filter) || Name.Contains(filter) || Type.Contains(filter);
        }
    }

    internal class ClubVipViewModel : IVipViewModel
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int MembersCount { get; private set; }
        public string Type { get { return "מועדון"; } }

        public ClubVipViewModel(int id, string name, int membersCount)
        {
            ID = id;
            Name = name;
            MembersCount = membersCount;
        }

        public bool Contains(string filter)
        {
            return ID.ToString().Contains(filter) || Name.Contains(filter) ||
                Type.Contains(filter) || MembersCount.ToString().Contains(filter);
        }
    }

    internal class VipAssociationViewModel : PopupViewModel
    {
        private int _groupID;
        private string _criteria;
        private List<PrivateVipViewModel> _availableCostumers;
        private List<ClubVipViewModel> _availableClubs;
        private List<IVipViewModel> _associatedVips;

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "שיוך ללקוחות",
                    Width = 570,
                    Height = 450
                };
            }
        }

        public string Criteria
        {
            get { return _criteria; }
            set
            {
                if (!SetProperty(ref _criteria, value)) return;
                OnPropertyChanged("AvailableCostumers");
                OnPropertyChanged("AvailableClubs");
                OnPropertyChanged("AssociatedVips");
            }
        }

        public List<PrivateVipViewModel> AvailableCostumers
        { get { return _availableCostumers.FindAll(vm => vm.Contains(Criteria)); } }
        public List<ClubVipViewModel> AvailableClubs
        { get { return _availableClubs.FindAll(vm => vm.Contains(Criteria)); } }
        public List<IVipViewModel> AssociatedVips
        { get { return _associatedVips.FindAll(vm => vm.Contains(Criteria)); } }

        public DelegateCommand<IVipViewModel> AssociateVipCommand { get; private set; }
        public DelegateCommand<IVipViewModel> DisassociateVipCommand { get; private set; }

        public VipAssociationViewModel(int groupID)
        {
            _groupID = groupID;
            _criteria = "";

            AssociateVipCommand = new DelegateCommand<IVipViewModel>(associateFunction);
            DisassociateVipCommand = new DelegateCommand<IVipViewModel>(disassociateFunction);

            populatePrivates();
            populateClubs();
            populateAssociated();
        }

        private void populatePrivates()
        {
            _availableCostumers = new List<PrivateVipViewModel>();
            var dt = DBService.GetService().GetVIPSingles(_groupID);
            foreach (DataRow r in dt.Rows)
                _availableCostumers.Add(new PrivateVipViewModel(
                    int.Parse(r["vipno"].ToString()), r["vname"].ToString()));
            OnPropertyChanged("AvailableCostumers");
        }

        private void populateClubs()
        {
            _availableClubs = new List<ClubVipViewModel>();
            var dt = DBService.GetService().GetVIPGroups(_groupID);
            foreach (DataRow r in dt.Rows)
                _availableClubs.Add(new ClubVipViewModel(int.Parse(r["clubno"].ToString()),
                    r["clubName"].ToString(), int.Parse(r["membersCount"].ToString())));
            OnPropertyChanged("AvailableClubs");
        }

        private void populateAssociated()
        {
            _associatedVips = new List<IVipViewModel>();
            var dt = DBService.GetService().GetSalesVIPs(_groupID);
            foreach (DataRow r in dt.Rows)
                _associatedVips.Add(bool.Parse(r["isVipno"].ToString())
                    ? (IVipViewModel)new PrivateVipViewModel(int.Parse(r["VipID"].ToString()), r["name"].ToString())
                    : new ClubVipViewModel(int.Parse(r["VipID"].ToString()), r["name"].ToString(),
                        int.Parse(r["membersCount"].ToString())));

            OnPropertyChanged("AssociatedVips");

        }

        private void associateFunction(IVipViewModel vm)
        {
            ActivityLogService.Logger.LogCall(vm.ID, vm is PrivateVipViewModel);
            try
            {
                DBService.GetService().AssociateVIP2Sale(_groupID, vm.ID, vm is PrivateVipViewModel);
                if (vm is PrivateVipViewModel)
                {
                    _availableCostumers.Remove(vm as PrivateVipViewModel);
                    _associatedVips.Add(vm);
                    OnPropertyChanged("AvailableCostumers");
                }
                else
                {
                    _availableClubs.Remove(vm as ClubVipViewModel);
                    _associatedVips.Add(vm);
                    OnPropertyChanged("AvailableClubs");
                }
                OnPropertyChanged("AssociatedVips");
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אירעה שגיאה בעת הוספת הלקוח למבצע, אנא נסו שוב מאוחר יותר.");
            }
        }

        private void disassociateFunction(IVipViewModel vm)
        {
            ActivityLogService.Logger.LogCall(vm.ID, vm is PrivateVipViewModel);
            try
            {
                DBService.GetService().DisassociateVIPfromSale(_groupID, vm.ID, vm is PrivateVipViewModel);
                if (vm is PrivateVipViewModel)
                {
                    _availableCostumers.Add(vm as PrivateVipViewModel);
                    _associatedVips.Remove(vm);
                    OnPropertyChanged("AvailableCostumers");
                }
                else
                {
                    _availableClubs.Add(vm as ClubVipViewModel);
                    _associatedVips.Remove(vm);
                    OnPropertyChanged("AvailableClubs");
                }
                OnPropertyChanged("AssociatedVips");
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אירעה שגיאה בעת הורדת הלקוח מהמבצע, אנא נסו שוב מאוחר יותר.");
            }
        }
    }
}
