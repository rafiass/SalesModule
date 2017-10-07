using SalesModule.Models;

namespace SalesModule.ViewModels
{
    internal class TargetPriceViewModel : SaleViewModel
    {
        //### Implement TargetPriceViewModel

        public TargetPriceViewModel() : base(null)
        {
            SetPopupTitle("מחיר מטרה");
        }

        protected override void LoadEmpty()
        {
        }
        protected override void LoadSale(SaleM s)
        {
        }
        protected override SaleM CreateSale()
        {
            return null;
        }
    }
}
