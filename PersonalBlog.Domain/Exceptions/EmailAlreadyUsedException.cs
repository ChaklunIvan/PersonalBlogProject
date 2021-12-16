
namespace PersonalBlog.Domain.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException() : base("Email is already in use!") { }
    }
}
