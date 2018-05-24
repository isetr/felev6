using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZH.Persistence
{
    public class Article
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        [ForeignKey("Course")]
        public Int32 CourseID { get; set; }

        public virtual Course Course { get; set; }

        [Required]
        public String File { get; set; }

        [Required]
        public System.DateTime Uploaded { get; set; }

        [Required]
        public Int32 Downloaded { get; set; }
    }
}
