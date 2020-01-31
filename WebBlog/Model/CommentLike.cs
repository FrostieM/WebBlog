using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebBlog.Model
{
    public class CommentLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [JsonIgnore]
        public User User { get; set; }
        
        [JsonIgnore]
        public Comment Comment { get; set; }
    }
}