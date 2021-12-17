
namespace PersonalBlog.Data.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException(string email) : base($"Email is already in use! {email}") { }
    }
}
