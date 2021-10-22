using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsStand.Core.ViewModels
{
    public class CustomerViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public ICollection<PurchaseViewModel> Purchases { get; set; }
    }
}
