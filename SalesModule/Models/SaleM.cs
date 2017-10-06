using System;
using System.Collections.Generic;

namespace SalesModule.Models
{
    internal class SaleM
    {
        public int SaleID { get; private set; }
        public string Title { get { return Properties.Title; } }
        public SaleTypes Type { get; private set; }
        public int Index { get; private set; }

        public SalesPropertiesM Properties { get; private set; }
        public List<ProdAmountM> ReqProducts { get; private set; }
        public List<DiscountedProductM> Discounted { get; private set; }
        public DiscountM Discount { get; private set; }

        public SaleM(SaleTypes type, SalesPropertiesM prop, List<ProdAmountM> Reqs,
            List<DiscountedProductM> discounteds, DiscountM TotalDiscount = null, int index = 1, int saleID = -1)
        {
            SaleID = saleID;
            Type = type;
            Index = index;

            Properties = prop;
            ReqProducts = Reqs ?? new List<ProdAmountM>();
            Discounted = discounteds ?? new List<DiscountedProductM>();
            Discount = TotalDiscount ?? new DiscountM(0, DiscountTypes.Nothing);
        }


        /// <summary>
        /// Main function for calculating sales
        /// </summary>
        /// <param name="bag"></param>
        /// List of remaining shoping items from the customer bag
        /// <param name="totalReceipt"></param>
        /// Total price of the bag
        /// <returns></returns>
        /// Retunrs a list of discount items 
         public List<SaleDiscount> GetEfective(List<ShoppingItem> bag, double totalReceipt)
        {
            double saleValue = 0, discountAmount, discountQTY;
            bool DiscountFound = true;
            int saleInstaceCounter = 1;
            List<SaleDiscount> discounts = new List<SaleDiscount>();
            List<ShoppingItem> discountBag, giftedBag;

            //Find if required elements for the sale exist in the bag
            //Do this for each instance of the sale
            var newBag = isRequiredExist(bag);
            if (newBag == null)
                return discounts;

            var plunosIn = new List<ShoppingItem>(newBag);
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
                    //Go over all discount elements are find them in the bag
                    foreach (var dp in Discounted)
                    {
                        discountQTY = 0;
                        if (dp.Amount == 0)
                        {
                            //the discount is for each 1 unit, limited by MaxMultiply
                            discountBag = findPlu(bag, dp.ID, dp.isPluno, 0, Properties.FavourOrder);
                            if (discountBag == null || discountBag.Count == 0) continue;

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
                            discountBag = findPlu(bag, dp.ID, dp.isPluno, dp.Amount, Properties.FavourOrder);
                            for (int k = 0; discountBag != null &&
                                (dp.MaxMultiply == 0 || k < dp.MaxMultiply); k++)
                            {
                                double tempValue = 0;
                                discountBag.ForEach(si => tempValue += si.Amount * si.Price);
                                discountAmount = dp.Discount.GetDiscount(tempValue);
                                discounts.Add(new SaleDiscount(SaleID, Title, 1, discountAmount, plunosIn));

                                discountQTY++;
                                if (k != dp.MaxMultiply - 1)
                                    discountBag = findPlu(bag, dp.ID, dp.isPluno, dp.Amount, Properties.FavourOrder);
                            }
                        }
                        if (discountQTY != 0)
                            DiscountFound = true;

                        foreach (var gift in dp.Discounted)
                        {
                            if (gift.Amount == 0)
                            {
                                //the discount is for each 1 unit, limited by discountQTY
                                giftedBag = findPlu(bag, gift.ID, gift.isPluno, 0, Properties.FavourOrder);
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
                                giftedBag = findPlu(bag, gift.ID, gift.isPluno, gift.Amount, Properties.FavourOrder);
                                for (int k = 0; k < discountQTY && giftedBag != null; k++)
                                {
                                    double tempValue = 0;
                                    giftedBag.ForEach(si => tempValue += si.Amount * si.Price);
                                    discountAmount = gift.Discount.GetDiscount(tempValue);
                                    discounts.Add(new SaleDiscount(SaleID, Title, 1, discountAmount, plunosIn));

                                    if (k != discountQTY - 1)
                                        giftedBag = findPlu(bag, gift.ID, gift.isPluno, gift.Amount, Properties.FavourOrder);
                                }
                            }
                        }
                    }
                }

                //Look for next instance of the sale
                newBag = isRequiredExist(bag);
                saleInstaceCounter++;
            }
            if (newBag != null)
                //Sale not active - return the required items back to the bag
                newBag.ForEach(si => Common.InsertItemToCart(bag, si));

            Common.collapseSales(discounts);
            return discounts;
        }
        public bool IsProductRequire(string pluno, int? kind)
        {
            return ReqProducts.Exists(p => (p.ID == pluno && p.isPluno) ||
                (kind != null && p.ID == kind.ToString() && !p.isPluno));
        }

        /// <summary>
        /// Find if required items for the sale exist in the bag
        /// For each required elemet, look for it in the bag
        /// If exist - remove it from the bag and add to the return list
        /// </summary>
        /// <param name="bag"></param>
        /// <returns></returns>
        /// List of shoping items that invoked the sale
        private List<ShoppingItem> isRequiredExist(List<ShoppingItem> bag)
        {
            var newBag = new List<ShoppingItem>();
            List<ShoppingItem> tempBag;
            foreach (ProdAmountM req in ReqProducts)
            {
                tempBag = findPlu(bag, req.ID, req.isPluno, req.Amount, searchOrder.highToLow);
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
        /// <summary>
        /// Search for an item in the bag, 
        /// if found retuns the items are remove it from the bag
        /// </summary>
        private List<ShoppingItem> findPlu(List<ShoppingItem> bag, string pluID, bool isPluno, double amount, searchOrder order)
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

            if (order == searchOrder.highToLow)
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
