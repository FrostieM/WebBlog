using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Model
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}