namespace MovieAPI.Services.Exceptions
{
    public class SignUpValidationException : Exception
    {
        public SignUpValidationException(string message)
            :base(message)
        {
            
        }
    }
}
