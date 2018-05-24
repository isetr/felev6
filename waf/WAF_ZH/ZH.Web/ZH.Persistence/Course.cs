using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZH.Persistence
{
    public class Course
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(64)]
        public String Name { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
