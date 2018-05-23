using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoorBash.Persistence.DTOs
{
    public class ItemDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public Int32 CategoryID { get; set; }

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
