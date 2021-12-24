
namespace PersonalBlog.Data.Exceptions
{
    public class NullableBlogException : Exception
    {
        public NullableBlogException() : base("blog is null") { }

    }
}
