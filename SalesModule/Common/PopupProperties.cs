namespace SalesModule
{
    internal class PopupProperties
    {
        public string Title { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public bool IsModal { get; set; }
        public bool IsShowingOnTaskBar { get; set; }

        public PopupProperties()
        {
            Title = "";
            Width = 300;
            Height = 300;
            IsModal = true;
            IsShowingOnTaskBar = false;
        }
    }
}
