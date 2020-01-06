using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebBlog.Model
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [JsonIgnore]
        public Blog Blog { get; set; }

        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        public string FileUrl { get; set; }
        
        [Required]
        public DateTime Created { get; set; }
        
        [JsonIgnore]
        public ICollection<PostLike> Likes { get; set; }
        
        [JsonIgnore]
        public ICollection<Tag> Tags { get; set; }
    }
}