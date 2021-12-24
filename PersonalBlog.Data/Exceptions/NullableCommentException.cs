namespace PersonalBlog.Data.Exceptions
{
    public class NullableCommentException : Exception
    {
        public NullableCommentException() : base("Comment is null")
        {

        }
    }
}
