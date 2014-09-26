using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataRepository.Models
{
    public class BlogTag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string TagValue { get; set; }

        public int? BlogPostID { get; set; }
        [ForeignKey("BlogPostID")]
        public virtual BlogPost BlogPost { get; set; }
    }
}
