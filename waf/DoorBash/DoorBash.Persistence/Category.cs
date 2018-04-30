using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DoorBash.Persistence
{
    public class Category
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(64)]
        public String Name { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
