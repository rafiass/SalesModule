using System.Collections.Generic;

namespace SalesModule.Models
{
    internal abstract class ComperableProductM
    {
        public string ID { get; private set; }
        public bool IsPluno { get; private set; }

        public ComperableProductM(string id, bool isProduct)
        {
            ID = id;
            IsPluno = isProduct;
        }

        public override bool Equals(object obj)
        {
            if (obj is ComperableProductM cp)
                return IsPluno == cp.IsPluno && ID == cp.ID;
            return false;
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
        public List<GiftedProductM> Gifted { get; private set; }

        public DiscountedProductM(string id, bool isProduct, double amount,
            double rec, DiscountM discount, GiftedProductM gift)
            : this(id, isProduct, amount, rec, discount)
        {
            if (gift != null)
                Gifted.Add(gift);
        }
        public DiscountedProductM(string id, bool isProduct, double amount,
            double rec, DiscountM discount, List<GiftedProductM> gifted = null, int outID = -1)
            : base(id, isProduct)
        {
            Amount = amount;
            MaxMultiply = rec;
            Discount = discount;
            Gifted = gifted ?? new List<GiftedProductM>();
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
