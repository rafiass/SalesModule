using System;
using System.ComponentModel;

namespace SalesModule.ViewModels
{
    internal abstract class PopupViewModel : ViewModelBase
    {
        protected bool IsClosing { get; set; }

        //internal can set but only protected can execute
        protected Action CloseWindow { get; private set; }
        public PopupProperties PopupProperties { get; protected set; }

        public PopupViewModel()
        {
            PopupProperties = new PopupProperties();
            CloseWindow = () => { };
            IsClosing = false;
        }

        protected internal virtual void WindowClosed()
        {
        }
        protected internal virtual void WindowClosing(CancelEventArgs e)
        {
        }
        internal void SetCloseAction(Action closeWin)
        {
            CloseWindow = () => { IsClosing = true; closeWin(); };
        }
        protected void SetPopupTitle(string title)
        {
            PopupProperties.Title = title;
        }
    }
}
