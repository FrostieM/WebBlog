using System.ComponentModel.DataAnnotations;

namespace WebBlog.Model.Forms
{
    public class CommentForm
    {
        [Required(ErrorMessage = "post id is required")]
        public int PostId { get; set; }
        
        [Required(ErrorMessage = "content is required")]
        public string Content { get; set; }
        public int? CommentId { get; set; }
    }
}