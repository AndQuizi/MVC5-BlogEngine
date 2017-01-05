using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogEngine6.Models
{
    public class Tag
    {
        public int TagID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }


        public virtual ICollection<Blog> Blog { get; set; }
    }
}