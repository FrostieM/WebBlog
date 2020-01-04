using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Model
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public Blog Blog { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        public string FileUrl { get; set; }
        
        public DateTime Created { get; set; }
        
        
        public ICollection<Tag> Tags { get; set; }
    }
}