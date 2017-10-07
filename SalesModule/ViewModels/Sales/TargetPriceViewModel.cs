using SalesModule.Models;

namespace SalesModule.ViewModels
{
    internal class TargetPriceViewModel : SaleViewModel
    {
        //### Implement TargetPriceViewModel

        public override PopupProperties PopupProperties
        {
            get
            {
                return new PopupProperties()
                {
                    Title = "מחיר מטרה",
                    Width = 300,
                    Height = 300
                };
            }
        }

        public TargetPriceViewModel() : base(null)
        {

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
