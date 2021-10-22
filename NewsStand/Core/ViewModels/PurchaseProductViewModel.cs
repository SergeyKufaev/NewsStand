using System.ComponentModel.DataAnnotations;

namespace NewsStand.Core.ViewModels
{
    public class PurchaseProductViewModel
    {
        public int? Id { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
