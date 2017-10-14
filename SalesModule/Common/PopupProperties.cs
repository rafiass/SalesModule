namespace SalesModule
{
    internal class PopupProperties
    {
        public string Title { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public double MinWidth { get; set; }
        public double MinHeight { get; set; }

        public bool IsModal { get; set; }
        public bool IsShowingOnTaskBar { get; set; }

        public PopupProperties()
        {
            Title = "";
            Width = 500;
            Height = 500;
            MinWidth = 400;
            MinHeight = 400;
            IsModal = true;
            IsShowingOnTaskBar = false;
        }
    }
}
