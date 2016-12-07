using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesModule.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        protected bool SetProperty<T>(ref T property, T val, [CallerMemberName] string propertyname = null)
        {
            if (property.Equals(val)) return false;

            property = val;
            if (!string.IsNullOrEmpty(propertyname))
                OnPropertyChanged(propertyname);
            return true;
        }
    }
}
