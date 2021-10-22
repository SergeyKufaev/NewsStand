using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NewsStand.Core.Entities;

namespace NewsStand.Core.ViewModels
{
    public class ProductViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public ProductType Type { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }

        public int ProducerId { get; set; }

        public ProducerViewModel Producer { get; set; }

        public List<int> AuthorsIds { get; set; }

        public List<AuthorViewModel> Authors { get; set; }

        [Required]
        public int NumberAvailable { get; set; }
    }
}
