using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DoorBash.Persistence
{
    public class Order
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Address { get; set; }

        [Required]
        public String Phone { get; set; }

        [Required]
        public Boolean Done { get; set; }

        [Required]
        public System.DateTime Sent { get; set; }

        public System.DateTime Approved { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }
}
