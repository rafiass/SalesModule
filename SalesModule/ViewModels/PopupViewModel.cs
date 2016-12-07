using System;

namespace SalesModule.ViewModels
{
    internal class PopupViewModel : ViewModelBase
    {
        //internal can set but only protected can execute
        protected Action CloseWindow { get; private set; }

        public PopupViewModel()
        {
            CloseWindow = () => { };
        }

        protected internal virtual void WindowClosed()
        {

        }
        internal void SetCloseAction(Action closeWin)
        {
            CloseWindow = closeWin;
        }
    }
}
