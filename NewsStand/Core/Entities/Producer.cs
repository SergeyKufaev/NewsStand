using System.Collections.Generic;

namespace NewsStand.Core.Entities
{
    public class Producer : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}