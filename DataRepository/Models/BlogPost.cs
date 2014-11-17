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

        [Required]
        public string Author { get; set; }
        [Required, MaxLength(50)]
        public string AuthorId { get; set; }
        [Required]
        public string BlogText { get; set; }
        [Required, MaxLength(100)]
        public string Category { get; set; }
        [Index, Required]
        public DateTime DateTimePosted { get; set; }
        [Index, Required]
        public DateTime DateTimeUpdated { get; set; }
        [Index, Required]
        public DateTime DateTimePublished { get; set; }
        public string MainImageId { get; set; }
        [Required]
        public string Title { get; set; }
        [Index(IsUnique=true), MaxLength(300), Required]
        public string UrlTitle { get; set; }
        
        // child items
        public virtual ICollection<BlogTag> Tags { get; set; }
        public virtual ICollection<BlogImage> Images { get; set; }
    }
}
