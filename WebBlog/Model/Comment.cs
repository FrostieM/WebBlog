using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebBlog.Model
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Post Post { get; set; }

        public User User { get; set; }
        
        public string Content { get; set; }

        [JsonIgnore]
        public Comment ParentComment { get; set; }
        
        public ICollection<Comment> SubComments { get; set; }
    }
}