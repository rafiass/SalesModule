using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesModule.ViewModels
{
    class SomeSaleViewModel
    {
        public string Title { get; set; }
        public int Counter { get; set; }

        public SomeSaleViewModel()
        {
            Title = "Fix!";
            Counter = 0;
        }
    }
}
