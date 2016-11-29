
using System.Collections.Generic;
namespace SalesModule
{
    internal interface IProduct
    {
        string ID { get; }
        string Name { get; }
        bool isPluno { get; }
    }

    internal class Product : IProduct
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Barcode { get; private set; }
        public int? Kind { get; private set; }
        public bool isPluno { get { return true; } }

        public Product(string id, string name, string barcode, int? kind = null)
        {
            ID = id;
            Name = name;
            Barcode = barcode;
            Kind = kind;
        }
    }

    internal class Category : IProduct
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Comments { get; private set; }
        public bool isPluno { get { return false; } }

        public Category(string id, string name, string rem)
        {
            ID = id;
            Name = name;
            Comments = rem;
        }
    }

    internal abstract class ComperableProduct
    {
        public string ID { get; private set; }
        public bool isPluno { get; private set; }

        public ComperableProduct(string id, bool isProduct)
        {
            ID = id;
            isPluno = isProduct;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ComperableProduct)) return false;
            var cp = obj as ComperableProduct;
            return isPluno == cp.isPluno && ID == cp.ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(ComperableProduct cp1, ComperableProduct cp2)
        {
            if (((object)cp1 == null) != ((object)cp2 == null)) return false;
            if ((object)cp1 == null) return true;
            return cp1.Equals(cp2);
        }
        public static bool operator !=(ComperableProduct cp1, ComperableProduct cp2)
        {
            return !(cp1 == cp2);
        }
    }

    internal class ProdAmount : ComperableProduct
    {
        public double Amount { get; set; }

        public ProdAmount(string id, bool isProduct, double amount)
            : base(id, isProduct)
        {
            Amount = amount;
        }
    }

    internal class DiscountedProduct : ComperableProduct
    {
        public int OutID { get; private set; }
        public double Amount { get; private set; }
        public double MaxMultiply { get; private set; }
        public Discount Discount { get; private set; }
        public List<GiftedProduct> Discounted { get; private set; }

        public DiscountedProduct(string id, bool isProduct, double amount,
            double rec, Discount discount, GiftedProduct gift)
            : this(id, isProduct, amount, rec, discount)
        {
            if (gift != null) Discounted.Add(gift);
        }
        public DiscountedProduct(string id, bool isProduct, double amount,
            double rec, Discount discount, List<GiftedProduct> gifted = null, int outID = -1)
            : base(id, isProduct)
        {
            Amount = amount;
            MaxMultiply = rec;
            Discount = discount;
            Discounted = gifted ?? new List<GiftedProduct>();
            OutID = outID;
        }
    }

    internal class GiftedProduct : ComperableProduct
    {
        public double Amount { get; private set; }
        public Discount Discount { get; private set; }

        public GiftedProduct(string id, bool isProduct,
            double amount, Discount discount)
            : base(id, isProduct)
        {
            Amount = amount;
            Discount = discount;
        }
        public GiftedProduct(IProduct prod, double amount, Discount discount)
            : this(prod.ID, prod.isPluno, amount, discount)
        { }
    }

}
