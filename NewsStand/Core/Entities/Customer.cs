using System.Collections.Generic;

namespace NewsStand.Core.Entities
{
    public class Customer : AuditableEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}