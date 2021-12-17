
namespace PersonalBlog.Data.Exceptions
{
    public class NullableUserException : Exception
    {
        public NullableUserException() : base("User is null!") { }
       
    }
}
