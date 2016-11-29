using System;
using System.Collections.Generic;

namespace SalesModule
{
    internal enum SaleTypes
    {
        None = 0, SingularDiscount, SingularLowerPrice,
        SingularBuyAndGet, AdvancedBuyAndGet, AdvancedBundle
    }

    internal class SalesGroup
    {
        public int GroupID { get; private set; }
        public UserData Emp { get; private set; }
        public DateTime DateCreated { get; private set; }
        public bool IsEnabled { get; private set; }

        public List<Sale> Sales { get; set; }

        public SalesGroup(Sale sale)
            : this(null, DateTime.Now, true, sale)
        { }
        public SalesGroup(UserData emp, DateTime created, bool isEnabled, Sale sale)
            : this(-1, emp, created, isEnabled)
        {
            Sales = new List<Sale>();
            Sales.Add(sale);
        }
        public SalesGroup(int groupID, UserData emp, DateTime created, bool isEnabled, List<Sale> sales = null)
        {
            GroupID = groupID;
            Sales = sales ?? new List<Sale>();
            Emp = emp;
            DateCreated = created;
            IsEnabled = isEnabled;
        }

        public List<SaleDiscount> GetEfective(List<ShoppingItem> bag, double totalReceipt)
        {
            List<SaleDiscount> sd;
            for (int i = 0; i < Sales.Count; i++)
                if ((sd = Sales[i].GetEfective(bag, totalReceipt)) != null)
                    return sd;
            return null;
        }

        public string IsProductRequire(string pluno, int? kind)
        {
            var sl = Sales.Find(s => s.IsProductRequire(pluno, kind));
            return sl != null ? sl.Title : "";
        }
    }

    internal class SalesProperties
    {
        public string Title { get; set; }
        public double MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int InstanceMultiply { get; set; }
        public int RecurrencePerInstance { get; set; }
        public bool IsBroadSale { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public SalesProperties()
            : this("מבצע") { }
        public SalesProperties(string title)
            : this(title, 0, null, 1, 1)
        { }
        public SalesProperties(string title, double MinPrice, double? maxPrice, int maxInstances, int maxRecurrences)
            : this(title, MinPrice, maxPrice, maxInstances, maxRecurrences,
             true, DateTime.Now, DateTime.Now + new TimeSpan(7, 0, 0, 0))
        { }
        public SalesProperties(string title, double minPrice, double? maxPrice,
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
        public SalesProperties(SalesProperties prop)
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

    internal class Sale
    {
        public int SaleID { get; private set; }
        public string Title { get { return Properties.Title; } }
        public SaleTypes Type { get; private set; }
        public int Index { get; private set; }

        public SalesProperties Properties { get; private set; }
        public List<ProdAmount> ReqProducts { get; private set; }
        public List<DiscountedProduct> Discounted { get; private set; }
        public Discount Discount { get; private set; }

        public Sale(SaleTypes type, SalesProperties prop, List<ProdAmount> Reqs,
            List<DiscountedProduct> discounteds, Discount TotalDiscount = null, int index = 1, int saleID = -1)
        {
            SaleID = saleID;
            Type = type;
            Index = index;

            Properties = prop;
            ReqProducts = Reqs ?? new List<ProdAmount>();
            Discounted = discounteds ?? new List<DiscountedProduct>();
            Discount = TotalDiscount ?? new Discount(0, DiscountTypes.Nothing);
        }

        public List<SaleDiscount> GetEfective(List<ShoppingItem> bag, double totalReceipt)
        {
            double saleValue = 0, discountAmount, discountQTY;
            bool DiscountFound = true;
            int saleInstaceCounter = 1;
            List<SaleDiscount> discounts = new List<SaleDiscount>();
            List<ShoppingItem> discountBag, giftedBag;

            var newBag = isRequiredExist(bag);
            if (newBag == null)
                return discounts;

            List<string> plunosIn = newBag.ConvertAll<string>(si => si.Pluno);
            while ((Properties.InstanceMultiply == 0 || saleInstaceCounter <= Properties.InstanceMultiply) &&
                (saleInstaceCounter * Properties.MinPrice <= totalReceipt) && newBag != null && DiscountFound)
            {
                saleValue = 0;
                newBag.ForEach(si => saleValue += si.Amount * si.Price);

                discounts.Add(new SaleDiscount(SaleID, Title, 1, Discount.GetDiscount(saleValue, totalReceipt), plunosIn));
                for (int i = 0; DiscountFound &&
                    (i < Properties.RecurrencePerInstance || Properties.RecurrencePerInstance == 0); i++)
                {
                    DiscountFound = false;
                    foreach (var dp in Discounted)
                    {
                        discountQTY = 0;
                        if (dp.Amount == 0)
                        {
                            //the discount is for each 1 unit, limited by MaxMultiply
                            discountBag = findPlu(bag, dp.ID, dp.isPluno, 0, false);
                            if (discountBag == null) continue;

                            double maxAllowed = 0;
                            if (dp.MaxMultiply == 0)
                                discountBag.ForEach(si => maxAllowed += si.Amount);
                            else
                                maxAllowed = dp.MaxMultiply;
                            discountQTY = maxAllowed;
                            int k;
                            for (k = 0; k < discountBag.Count; k++)
                            {
                                discountAmount = dp.Discount.GetDiscount(discountBag[k].Price);
                                if (discountBag[k].Amount <= maxAllowed)
                                {
                                    discounts.Add(new SaleDiscount(SaleID, Title, discountBag[k].Amount, discountAmount, plunosIn));
                                    maxAllowed -= discountBag[k].Amount;
                                    discountBag.RemoveAt(k--);
                                }
                                else
                                {
                                    discounts.Add(new SaleDiscount(SaleID, Title, maxAllowed, discountAmount, plunosIn));
                                    if (discountBag[k].Reduce(maxAllowed) == 0)
                                        discountBag.RemoveAt(k--);
                                    break;
                                }
                            }
                            for (; k < discountBag.Count; k++)//return everything else back to the cart
                                Common.InsertItemToCart(bag, discountBag[k]);
                            discountBag.Clear();
                        }
                        else
                        {
                            //the discount is for each full package of dp.Amount units, limited by MaxMultiply packages
                            discountBag = findPlu(bag, dp.ID, dp.isPluno, dp.Amount, false);
                            for (int k = 0; discountBag != null &&
                                (dp.MaxMultiply == 0 || k < dp.MaxMultiply); k++)
                            {
                                double tempValue = 0;
                                discountBag.ForEach(si => tempValue += si.Amount * si.Price);
                                discountAmount = dp.Discount.GetDiscount(tempValue);
                                discounts.Add(new SaleDiscount(SaleID, Title, 1, discountAmount, plunosIn));

                                discountQTY++;
                                if (k != dp.MaxMultiply - 1)
                                    discountBag = findPlu(bag, dp.ID, dp.isPluno, dp.Amount, false);
                            }
                        }
                        if (discountQTY != 0)
                            DiscountFound = true;

                        foreach (var gift in dp.Discounted)
                        {
                            if (gift.Amount == 0)
                            {
                                //the discount is for each 1 unit, limited by discountQTY
                                giftedBag = findPlu(bag, gift.ID, gift.isPluno, 0, false);
                                if (giftedBag == null) continue;

                                double maxAllowed = discountQTY;
                                int k;
                                for (k = 0; k < giftedBag.Count; k++)
                                {
                                    discountAmount = gift.Discount.GetDiscount(giftedBag[k].Price);
                                    if (giftedBag[k].Amount <= maxAllowed)
                                    {
                                        discounts.Add(new SaleDiscount(SaleID, Title, giftedBag[k].Amount, discountAmount, plunosIn));
                                        maxAllowed -= giftedBag[k].Amount;
                                    }
                                    else
                                    {
                                        discounts.Add(new SaleDiscount(SaleID, Title, maxAllowed, discountAmount, plunosIn));
                                        Common.InsertItemToCart(bag, new ShoppingItem(giftedBag[k].Pluno,
                                            giftedBag[k].Amount - maxAllowed, giftedBag[k].Price));
                                        if (giftedBag[k].Reduce(maxAllowed) == 0)
                                            giftedBag.RemoveAt(k--);
                                        break;
                                    }
                                }
                                for (; k < giftedBag.Count; k++)//return everything else back to the cart
                                    Common.InsertItemToCart(bag, giftedBag[k]);
                                giftedBag.Clear();
                            }
                            else
                            {
                                //the discount is for each full package of gift.Amount units,
                                //limited by discountQTY
                                giftedBag = findPlu(bag, gift.ID, gift.isPluno, gift.Amount, false);
                                for (int k = 0; k < discountQTY && giftedBag != null; k++)
                                {
                                    double tempValue = 0;
                                    giftedBag.ForEach(si => tempValue += si.Amount * si.Price);
                                    discountAmount = gift.Discount.GetDiscount(tempValue);
                                    discounts.Add(new SaleDiscount(SaleID, Title, 1, discountAmount, plunosIn));

                                    if (k != discountQTY - 1)
                                        giftedBag = findPlu(bag, gift.ID, gift.isPluno, gift.Amount, false);
                                }
                            }
                        }
                    }
                }

                newBag = isRequiredExist(bag);
                saleInstaceCounter++;
            }
            if (newBag != null)
                newBag.ForEach(si => Common.InsertItemToCart(bag, si));

            Common.collapseSales(discounts);
            return discounts;
        }
        public bool IsProductRequire(string pluno, int? kind)
        {
            return ReqProducts.Exists(p => (p.ID == pluno && p.isPluno) ||
                (kind != null && p.ID == kind.ToString() && !p.isPluno));
        }

        private List<ShoppingItem> isRequiredExist(List<ShoppingItem> bag)
        {
            var newBag = new List<ShoppingItem>();
            List<ShoppingItem> tempBag;
            foreach (ProdAmount req in ReqProducts)
            {
                tempBag = findPlu(bag, req.ID, req.isPluno, req.Amount, true);
                if (tempBag != null)
                    tempBag.ForEach(si => Common.InsertItemToCart(newBag, si));
                else
                {
                    newBag.ForEach(si => Common.InsertItemToCart(bag, si));
                    return null;
                }
            }
            return newBag;
        }
        private List<ShoppingItem> findPlu(List<ShoppingItem> bag, string pluID, bool isPluno, double amount, bool isReq)
        {
            var found = new List<ShoppingItem>();
            bool infinite = amount == 0;

            //in: the index of the compared item
            //out: is that item reduced completely or not
            Func<int, bool> search = i =>
            {
                ShoppingItem si = bag[i];
                double toMove = 0;
                if ((isPluno && si.Pluno == pluID) ||
                    (!isPluno && (si.Kind != null && si.Kind.ToString() == pluID)))
                {
                    toMove = infinite ? si.Amount : Math.Min(si.Amount, amount);
                    Common.InsertItemToCart(found, new ShoppingItem(pluID, toMove, si.Price));
                    if (!infinite) amount -= toMove;
                    return bag[i].Reduce(toMove) == 0;
                }
                return false;
            };

            if (isReq)
            {
                for (int i = bag.Count - 1; i >= 0; i--)//highest to lowest
                {
                    if (search(i))
                        bag.RemoveAt(i);

                    if (amount == 0 && !infinite)
                        return found;
                }
            }
            else
            {
                for (int i = 0; i < bag.Count; i++)//lowest to highest
                {
                    if (search(i))
                    {
                        bag.RemoveAt(i);
                        if (bag.Count != i)
                            i--;
                    }
                    if (amount == 0 && !infinite)
                        return found;
                }
            }
            if (infinite) return found;
            //if you reached here, not enough items found
            found.ForEach(si => Common.InsertItemToCart(bag, si));
            return null;
        }
    }
}
