using System.Collections.Generic;

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

    internal abstract class ComperableProductM
    {
        public string ID { get; private set; }
        public bool isPluno { get; private set; }

        public ComperableProductM(string id, bool isProduct)
        {
            ID = id;
            isPluno = isProduct;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ComperableProductM)) return false;
            var cp = obj as ComperableProductM;
            return isPluno == cp.isPluno && ID == cp.ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(ComperableProductM cp1, ComperableProductM cp2)
        {
            if (((object)cp1 == null) != ((object)cp2 == null)) return false;
            if ((object)cp1 == null) return true;
            return cp1.Equals(cp2);
        }
        public static bool operator !=(ComperableProductM cp1, ComperableProductM cp2)
        {
            return !(cp1 == cp2);
        }
    }

    internal class ProdAmountM : ComperableProductM
    {
        public double Amount { get; set; }

        public ProdAmountM(string id, bool isProduct, double amount)
            : base(id, isProduct)
        {
            Amount = amount;
        }
    }

    internal class DiscountedProductM : ComperableProductM
    {
        public int OutID { get; private set; }
        public double Amount { get; private set; }
        public double MaxMultiply { get; private set; }
        public DiscountM Discount { get; private set; }
        public List<GiftedProductM> Discounted { get; private set; }

        public DiscountedProductM(string id, bool isProduct, double amount,
            double rec, DiscountM discount, GiftedProductM gift)
            : this(id, isProduct, amount, rec, discount)
        {
            if (gift != null) Discounted.Add(gift);
        }
        public DiscountedProductM(string id, bool isProduct, double amount,
            double rec, DiscountM discount, List<GiftedProductM> gifted = null, int outID = -1)
            : base(id, isProduct)
        {
            Amount = amount;
            MaxMultiply = rec;
            Discount = discount;
            Discounted = gifted ?? new List<GiftedProductM>();
            OutID = outID;
        }
    }

    internal class GiftedProductM : ComperableProductM
    {
        public double Amount { get; private set; }
        public DiscountM Discount { get; private set; }

        public GiftedProductM(string id, bool isProduct,
            double amount, DiscountM discount)
            : base(id, isProduct)
        {
            Amount = amount;
            Discount = discount;
        }
        public GiftedProductM(IProductM prod, double amount, DiscountM discount)
            : this(prod.ID, prod.isPluno, amount, discount)
        { }
    }

}
