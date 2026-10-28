namespace MovieAPI.Services.Exceptions
{
    public class ReviewValidationException : Exception
    {
        public ReviewValidationException(string message)
            :base(message)
        {
            
        }
    }
}
