namespace SalesModule
{
    internal class PopupProperties
    {
        public string Title { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public double MinWidth { get; set; }
        public double MinHeigth { get; set; }

        public bool IsModal { get; set; }
        public bool IsShowingOnTaskBar { get; set; }

        public PopupProperties()
        {
            Title = "";
            Width = 400;
            Height = 400;
            MinWidth = 300;
            MinHeigth = 300;
            IsModal = true;
            IsShowingOnTaskBar = false;
        }
    }
}
