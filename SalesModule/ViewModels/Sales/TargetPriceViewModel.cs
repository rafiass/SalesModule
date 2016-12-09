using SalesModule.Models;

namespace SalesModule.ViewModels
{
    internal class TargetPriceViewModel : SaleViewModel
    {
        //### TargetPriceViewModel

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

        public TargetPriceViewModel()
        {
            
        }
        
        protected override void LoadSale()
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
