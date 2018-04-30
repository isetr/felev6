using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DoorBash.Persistence;

namespace DoorBash.WebSite.Models
{
    public class OrderViewModel
    {
        [Required]
        [DisplayName("Full Name")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required]
        [DisplayName("Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
