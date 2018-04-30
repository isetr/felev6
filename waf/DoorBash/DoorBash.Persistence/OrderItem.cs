using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DoorBash.Persistence
{
    public class OrderItem
    {
        [Key]
        public Int32 ID { get; set; }

        [Required]
        [ForeignKey("Order")]
        public Int32 OrderID { get; set; }

        public Order Order { get; set; }

        [Required]
        [ForeignKey("Item")]
        public Int32 ItemID { get; set; }

        public Item Item { get; set; }
    }
}
