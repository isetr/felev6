using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZH.Persistence
{
    public class Rating
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [ForeignKey("Article")]
        public Int32 ArticleID { get; set; }

        public virtual Article Article { get; set; }

        public ICollection<ArticleRating> ArticleRating { get; set; }
    }
}
