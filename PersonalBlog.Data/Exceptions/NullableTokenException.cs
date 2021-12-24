namespace PersonalBlog.Data.Exceptions
{
    public class NullableTokenException : Exception
    {
        public NullableTokenException() 
            : base("There is no tokens!") { }
    }
}
