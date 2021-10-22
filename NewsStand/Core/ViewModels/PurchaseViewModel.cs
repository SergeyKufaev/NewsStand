using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsStand.Core.ViewModels
{
    public class PurchaseViewModel
    {
        public int? Id { get; set; }
        public int CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public List<PurchaseProductViewModel> PurchaseProducts { get; set; }
        public decimal TotalPrice => PurchaseProducts.Select(p => p.Price * p.Quantity).Sum();
        public DateTime Date { get; set; } = DateTime.Now.Date;
    }
}
