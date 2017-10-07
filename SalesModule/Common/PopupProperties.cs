namespace SalesModule
{
    internal class PopupProperties
    {
        public string Title { get; set; }
        
        public bool IsModal { get; set; }
        public bool IsShowingOnTaskBar { get; set; }

        public PopupProperties()
        {
            Title = "";
            IsModal = true;
            IsShowingOnTaskBar = false;
        }
    }
}
