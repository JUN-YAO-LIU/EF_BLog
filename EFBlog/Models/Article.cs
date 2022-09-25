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
        public string Title { get; set; }

        public string? ArticleContent { get; set; }

        public bool IsDelete { get; set; }

        [ForeignKey("Account")]
        public string UserId { get; set; }

        public AuthUser AuthUser { get; set; }
    }
}