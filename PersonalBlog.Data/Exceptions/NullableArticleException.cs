namespace PersonalBlog.Data.Exceptions
{
    public class NullableArticleException : Exception
    {
        public NullableArticleException() : base("Article is null")
        {

        }
    }
}
