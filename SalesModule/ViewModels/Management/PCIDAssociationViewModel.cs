using SalesModule.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace SalesModule.ViewModels
{
    internal class PCIDViewModel : ViewModelBase
    {
        private int _groupID;
        private bool _isCreated;
        private bool _isEnabled;
        private string _bhname;
        private bool _isHoursLimited, _isDatesLimited;
        private int _timIndexFrom, _timeIndexTo;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isCreated && SetProperty(ref _isEnabled, value))
                    DBService.GetService().DisablePCID(_groupID, BranchNo, value);
            }
        }

        public List<Tuple<string, int>> Branches { get; private set; }
        public int BranchNo { get; set; }
        public string BranchName
        {
            get { return _isCreated ? _bhname : Branches.Find(b => b.Item2 == BranchNo).Item1; }
            set { if (_isCreated) SetProperty(ref _bhname, value); }
        }

        public DateTime DateFrom { get; set; }

        public bool IsDatesLimited
        {
            get { return _isDatesLimited; }
            set { SetProperty(ref _isDatesLimited, value); }
        }
        public DateTime? DateTo { get; set; }

        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public bool IsHoursLimited
        {
            get { return _isHoursLimited; }
            set { SetProperty(ref _isHoursLimited, value); }
        }

        public List<string> Hours { get; private set; }
        public int FromIndexTime
        {
            get { return _timIndexFrom; }
            set
            {
                if (!SetProperty(ref _timIndexFrom, value)) return;
                FromTime = indexToTime(value);
                OnPropertyChanged("FromTime");
            }
        }
        public int ToIndexTime
        {
            get { return _timeIndexTo; }
            set
            {
                if (!SetProperty(ref _timeIndexTo, value)) return;
                ToTime = indexToTime(value);
                OnPropertyChanged("ToTime");
            }
        }

        public PCIDViewModel(int groupID, bool isEnabled, bool isReal)
        {
            _groupID = groupID;
            _isEnabled = isEnabled;
            _isCreated = isReal;
            _bhname = null;

            populateBranches();
            populateHours();

            IsDatesLimited = true;
            DateFrom = DateTime.Today;
            DateTo = DateFrom + TimeSpan.FromDays(7); // a week from today

            IsHoursLimited = false;
            FromIndexTime = 0;
            ToIndexTime = 48;
        }

        internal void populateBranches()
        {
            if (_isCreated) return;

            var dt = DBService.GetService().GetUnattachedPcid(_groupID);
            var r = dt.NewRow();
            r["bhname"] = "בחר סניף";
            r["bhno"] = -1;
            dt.Rows.InsertAt(r, 0);

            Branches = new List<Tuple<string, int>>();
            foreach (DataRow branch in dt.Rows)
                Branches.Add(new Tuple<string, int>(
                    branch["bhname"].ToString(), int.Parse(branch["bhno"].ToString())));
            BranchNo = -1;
        }
        private void populateHours()
        {
            Hours = new List<string>();
            for (int i = 0; i < 48; i++)
                Hours.Add((i / 2 < 10 ? "0" : "") + (i / 2) + (i % 2 == 0 ? ":00" : ":30"));
            Hours.Add("23:59");
        }

        private TimeSpan indexToTime(int index)
        {
            return new TimeSpan(0, 30 * index, 0) -
                   (index == 48 ? new TimeSpan(0, 0, 1) : new TimeSpan(0, 0, 0));
        }
    }

    internal class PCIDAssociationViewModel : PopupViewModel
    {
        private int _groupID;
        private PCIDViewModel _newPcid;

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "שיוך לסניפים",
                    Width = 850,
                    MinWidth = 700,
                    Height = 500,
                    MinHeigth = 400
                };
            }
        }

        public ObservableCollection<PCIDViewModel> Associations { get; private set; }
        public PCIDViewModel NewPcid
        {
            get { return _newPcid; }
            set { SetProperty(ref _newPcid, value); }
        }

        public DelegateCommand AssociateCommand { get; private set; }
        public DelegateCommand<PCIDViewModel> DisassociateCommand { get; private set; }

        public PCIDAssociationViewModel(int groupID)
        {
            _groupID = groupID;
            AssociateCommand = new DelegateCommand(associateFunction);
            DisassociateCommand = new DelegateCommand<PCIDViewModel>(disassociateFunction);

            NewPcid = new PCIDViewModel(_groupID, true, false);
            populateAssociations();
        }

        private void populateAssociations()
        {
            Associations = new ObservableCollection<PCIDViewModel>();
            var dt = DBService.GetService().GetSalesBranches(_groupID);
            foreach (DataRow r in dt.Rows)
            {
                var ass = new PCIDViewModel(_groupID, bool.Parse(r["isEnabled"].ToString()), true);
                ass.BranchNo = int.Parse(r["pcid"].ToString());
                ass.BranchName = r["bhname"].ToString();

                ass.DateFrom = DateTime.Parse(r["dateFrom"].ToString());
                ass.IsDatesLimited = r["dateTo"] != DBNull.Value;
                ass.DateTo = ass.IsDatesLimited ? DateTime.Parse(r["dateTo"].ToString()) : (DateTime?)null;

                ass.IsHoursLimited = r["HourFrom"] != DBNull.Value;
                ass.FromTime = ass.IsHoursLimited ? TimeSpan.Parse(r["HourFrom"].ToString()) : (TimeSpan?)null;
                ass.ToTime = ass.IsHoursLimited ? TimeSpan.Parse(r["HourTo"].ToString()) : (TimeSpan?)null;

                Associations.Add(ass);
            }
            OnPropertyChanged("Associations");
        }
        private void associateFunction()
        {
            ActivityLogService.Logger.LogCall();
            try
            {
                if (NewPcid.BranchNo == -1)
                    MessageBox.Show("אנא בחר סניף");
                else if (NewPcid.IsDatesLimited &&
                         (NewPcid.DateTo < NewPcid.DateFrom || NewPcid.DateTo < DateTime.Now))
                    MessageBox.Show("טווח התאריכים אינו תקין");
                else if (NewPcid.IsHoursLimited && NewPcid.ToTime <= NewPcid.FromTime)
                    MessageBox.Show("טווח השעות אינו תקין");
                else
                {
                    if (NewPcid.FromTime == null || NewPcid.ToTime == null)
                        NewPcid.IsHoursLimited = false;

                    DateTime? to = NewPcid.IsDatesLimited ? NewPcid.DateTo : (DateTime?)null;
                    bool added;
                    if (NewPcid.IsHoursLimited)
                        added = DBService.GetService().AssociatePcid2SaleM(_groupID, NewPcid.BranchNo,
                            NewPcid.DateFrom, to, (TimeSpan)NewPcid.FromTime, (TimeSpan)NewPcid.ToTime);
                    else
                        added = DBService.GetService().AssociatePcid2SaleM(_groupID, NewPcid.BranchNo, NewPcid.DateFrom, to);

                    if (!added)
                        throw new SalesException("");

                    NewPcid = new PCIDViewModel(_groupID, true, false);
                    populateAssociations();
                }
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אירעה שגיאה בעת הוספת המבצע לסניף, אנא בדוק את הערכים שהוכנסו.");
            }
        }

        private void disassociateFunction(PCIDViewModel pcid)
        {
            ActivityLogService.Logger.LogCall();
            try
            {
                if (!DBService.GetService().DisassociatePcidfromSaleM(_groupID, pcid.BranchNo))
                    throw new SalesException("מחיקת סניף נכשלה");
                Associations.Remove(pcid);
                OnPropertyChanged("Associations");
                NewPcid = new PCIDViewModel(_groupID, true, false);
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אירעה שגיאה בעת מחיקת המבצע מהסניף, אנא פנה אל מרכז התמיכה.\nבינתיים - בטל את הסניף.");
            }

        }
    }
}
