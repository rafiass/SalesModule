using System;

namespace SalesModule.Models
{
    internal class SalesPropertiesM
    {
        public string Title { get; set; }
        public double MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int InstanceMultiply { get; set; }
        public int RecurrencePerInstance { get; set; }
        public bool IsBroadSale { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public SalesPropertiesM()
            : this("מבצע") { }
        public SalesPropertiesM(string title)
            : this(title, 0, null, 1, 1)
        { }
        public SalesPropertiesM(string title, double MinPrice, double? maxPrice, int maxInstances, int maxRecurrences)
            : this(title, MinPrice, maxPrice, maxInstances, maxRecurrences,
             true, DateTime.Now, DateTime.Now + new TimeSpan(7, 0, 0, 0))
        { }
        public SalesPropertiesM(string title, double minPrice, double? maxPrice,
            int maxInstances, int maxRecurrences, bool IsBroad, DateTime from, DateTime? to)
        {
            Title = title;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            InstanceMultiply = maxInstances;
            RecurrencePerInstance = maxRecurrences;
            IsBroadSale = IsBroad;
            DateFrom = from;
            DateTo = to;
        }
        public SalesPropertiesM(SalesPropertiesM prop)
        {
            Title = prop.Title;
            MinPrice = prop.MinPrice;
            MaxPrice = prop.MaxPrice;
            InstanceMultiply = prop.InstanceMultiply;
            RecurrencePerInstance = prop.RecurrencePerInstance;
            IsBroadSale = prop.IsBroadSale;
            DateFrom = prop.DateFrom;
            DateTo = prop.DateTo;
        }
    }
}
