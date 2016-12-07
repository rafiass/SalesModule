using System;
using System.Collections;
using System.Collections.Generic;
using SalesModule.Models;
using SalesModule.Services;

namespace SalesModule.Controls
{
    internal class ProductsSuggestionProvider : ISuggestionProvider
    {
        private List<IProductM> _products;

        public ProductsSuggestionProvider()
        {
            _products = DBService.GetService().GetProducts();
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

            foreach (var item in _products)
                if (isContain(item.Name, filter) ||
                    (item is ProductM && isContain((item as ProductM).Barcode, filter)))
                    yield return item;
        }
    }
}
