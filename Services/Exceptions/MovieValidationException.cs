namespace MovieAPI.Services.Exceptions
{
    public class MovieValidationException : Exception
    {
        public MovieValidationException(string message)
             : base(message)
        {
            
        }
    }
}
