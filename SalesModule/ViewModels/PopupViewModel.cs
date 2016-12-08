using System;
using System.ComponentModel;

namespace SalesModule.ViewModels
{
    internal abstract class PopupViewModel : ViewModelBase
    {
        //internal can set but only protected can execute
        protected Action CloseWindow { get; private set; }
        public abstract PopupProperties PopupProperties { get; }

        public PopupViewModel()
        {
            CloseWindow = () => { };
        }

        protected internal virtual void WindowClosed()
        {
        }
        protected internal virtual void WindowClosing(CancelEventArgs e)
        {
        }
        internal void SetCloseAction(Action closeWin)
        {
            CloseWindow = closeWin;
        }
    }
}
