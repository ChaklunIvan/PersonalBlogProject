
namespace PersonalBlog.Data.Exceptions
{
    public class UserNameAlreadyUsedException : Exception
    {
        public UserNameAlreadyUsedException(string userName) 
            : base($"User name already in use! {userName}") { }
    }
}
