using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFBlog.Models
{
    public class Article
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Title { get; set; } = string.Empty;

        public string ArticleContent { get; set; } = string.Empty;

        public bool IsDelete { get; set; }
    }
}