using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebBlog.Model
{
    public class PostTags
    {
        [Key]
        public int Id { get; set; }
        
        [JsonIgnore]
        public Post Post { get; set; }
        
        [JsonIgnore]
        public Tag Tag { get; set; }
        
    }
}