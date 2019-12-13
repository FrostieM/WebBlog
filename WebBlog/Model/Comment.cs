using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Model
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Post Post { get; set; }
        public Comment UpComment { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
    }
}