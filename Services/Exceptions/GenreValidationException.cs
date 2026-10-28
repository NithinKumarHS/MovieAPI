namespace MovieAPI.Services.Exceptions
{
    public class GenreValidationException : Exception
    {
        public GenreValidationException(string message)
            :base(message)
        {
            
        }
    }
}
