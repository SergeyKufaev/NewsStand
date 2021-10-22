using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsStand.Core.ViewModels
{
    public class ProducerViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<int> ProductsIds { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
    }
}
