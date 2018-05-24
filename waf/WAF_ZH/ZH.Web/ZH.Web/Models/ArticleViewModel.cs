using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using ZH.Persistence;

namespace ZH.Web.Models
{
    public class ArticleViewModel
    {
        [DisplayName("Title")]
        public string Name { get; set; }
        
        [DisplayName("Course")]
        public int CourseID { get; set; }
        
        [DisplayName("File")]
        public IList<IFormFile> File { get; set; }
    }
}
