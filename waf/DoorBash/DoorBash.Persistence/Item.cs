using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DoorBash.Persistence
{
    public class Item
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        [ForeignKey("Category")]
        public Int32 CategoryID { get; set; }

        public virtual Category Category { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [Required]
        public Int32 Price { get; set; }

        [Required]
        public Boolean Hot { get; set; }

        [Required]
        public Boolean Vegan { get; set; }
    }
}
