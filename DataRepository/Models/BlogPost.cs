using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataRepository.Models
{
    public class BlogPost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [Index, Required]
        public DateTime DateTimePosted { get; set; }
        public string MainImageId { get; set; }
        public string BlogText { get; set; }
        [Index(IsUnique=true), MaxLength(300), Required]
        public string UrlTitle { get; set; }
        
        public virtual ICollection<BlogTag> Tags { get; set; }
        public virtual ICollection<BlogImage> Images { get; set; }
    }
}
