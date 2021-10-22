using System.Collections.Generic;

namespace NewsStand.Core.Entities
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ProductType Type { get; set; }
        public decimal Price { get; set; }
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ICollection<AuthorProduct> AuthorProducts { get; set; }
        public int NumberAvailable { get; set; }
    }
}
