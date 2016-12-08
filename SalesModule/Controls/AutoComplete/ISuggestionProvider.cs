using System;
using System.Collections;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.Controls
{
    internal interface ISuggestionProvider
    {
        IEnumerable GetSuggestions(string filter);
    }

    internal class ProductsSuggestionProvider : ISuggestionProvider
    {
        private List<IProductM> _products;
        private List<IProductM> Products
        {
            get
            {
                try
                {
                    if (_products != null) return _products;
                    return _products = DBService.GetService().GetProducts();
                }
                catch
                {
                    return _products = null;
                }
            }
        }

        public ProductsSuggestionProvider()
        {
        }

        public IEnumerable GetSuggestions(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                yield break;

            Func<string, string, bool> isContain = (searchIn, searchFor) =>
            {
                if (searchFor.Length > searchIn.Length) return false;
                return searchIn.ToLower().Substring(0, searchFor.Length) == searchFor.ToLower();
            };

            foreach (var item in Products)
                if (isContain(item.Name, filter) ||
                    (item is ProductM && isContain((item as ProductM).Barcode, filter)))
                    yield return item;
        }
    }
}
