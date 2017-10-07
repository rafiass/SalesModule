
namespace SalesModule.Models
{
    internal interface IProductM
    {
        string ID { get; }
        string Name { get; }
        bool isPluno { get; }

        bool Contains(string filter);
    }

    internal class ProductM : IProductM
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Barcode { get; private set; }
        public int? Kind { get; private set; }
        public bool isPluno { get { return true; } }

        public ProductM(string id, string name, string barcode, int? kind = null)
        {
            ID = id;
            Name = name;
            Barcode = barcode;
            Kind = kind;
        }

        public bool Contains(string filter)
        {
            return ID.Contains(filter) || Name.Contains(filter) ||
                (Kind != null && Kind.ToString().Contains(filter));
        }
    }

    internal class CategoryM : IProductM
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Comments { get; private set; }
        public bool isPluno { get { return false; } }

        public CategoryM(string id, string name, string rem)
        {
            ID = id;
            Name = name;
            Comments = rem;
        }

        public bool Contains(string filter)
        {
            return ID.Contains(filter) || Name.Contains(filter) ||
                Comments.Contains(filter);
        }
    }
}
