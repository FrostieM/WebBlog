namespace WebBlog.Model.Forms
{
    public class SignUpForm : LoginForm
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }
}