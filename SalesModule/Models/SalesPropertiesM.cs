using System;

namespace SalesModule.Models
{
    internal class SalesPropertiesM
    {
        public double MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int InstanceMultiply { get; set; }
        public int RecurrencePerInstance { get; set; }

        public bool IsBroadSale { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        //Define how to go thorug the discount list
        public searchOrder FavourOrder { get; private set; }
        
        public SalesPropertiesM()
            : this(0, null, 1, 1)
        { }
        public SalesPropertiesM(double MinPrice, double? maxPrice, int maxInstances, int maxRecurrences)
            : this(MinPrice, maxPrice, maxInstances, maxRecurrences, true, DateTime.Now, null)
        { }
        public SalesPropertiesM(double minPrice, double? maxPrice,
            int maxInstances, int maxRecurrences, bool IsBroad, DateTime from, DateTime? to)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            InstanceMultiply = maxInstances;
            RecurrencePerInstance = maxRecurrences;
            IsBroadSale = IsBroad;
            DateFrom = from;
            DateTo = to;

            //TODO - add to DB as property
            FavourOrder = searchOrder.highToLow;
        }
        public SalesPropertiesM(SalesPropertiesM prop)
        {
            MinPrice = prop.MinPrice;
            MaxPrice = prop.MaxPrice;
            InstanceMultiply = prop.InstanceMultiply;
            RecurrencePerInstance = prop.RecurrencePerInstance;
            IsBroadSale = prop.IsBroadSale;
            DateFrom = prop.DateFrom;
            DateTo = prop.DateTo;
            FavourOrder = prop.FavourOrder;
        }
    }
}
