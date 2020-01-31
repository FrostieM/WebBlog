using System;
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
        
        [Required]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public Comment ParentComment { get; set; }
        
        [JsonIgnore]
        public ICollection<CommentLike> Likes { get; set; }
        
        [JsonIgnore]
        public ICollection<Comment> SubComments { get; set; }
        
    }
}