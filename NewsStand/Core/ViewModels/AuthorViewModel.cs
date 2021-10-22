using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsStand.Core.ViewModels
{
    public class AuthorViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public List<int> ProductsIds { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
    }
}
