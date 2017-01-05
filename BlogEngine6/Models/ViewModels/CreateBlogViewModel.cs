using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.Models.ViewModels
{
    public class CreateBlogViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [StringLength(5000, MinimumLength = 10)]
        public string Content { get; set; }
    }
}