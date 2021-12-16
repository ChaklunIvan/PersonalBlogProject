
namespace PersonalBlog.Domain.Exceptions
{
    public class UserNameAlreadyUsedException : Exception
    {
        public UserNameAlreadyUsedException() : base("User name already in use!") { }
    }
}
