using System;
using System.Collections.Generic;

namespace NewsStand.Core.Entities
{
    public class Purchase : AuditableEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<PurchaseProduct> PurchaseProducts { get; set; }
        public DateTime Date { get; set; }
    }
}