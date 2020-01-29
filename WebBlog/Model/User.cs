using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebBlog.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public string Id { get; set; }
        
        [JsonIgnore]
        public Blog Blog { get; set; }
        
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(20)]
        [JsonIgnore]
        public string Password { get; set; }
        
        [Required]
        [StringLength(40)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(40)]
        public string LastName { get; set; }
        
        [Required]
        [StringLength(40)]
        public string Email { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }
        
        [JsonIgnore]
        public ICollection<PostLike> Likes { get; set; }
        
    }
}