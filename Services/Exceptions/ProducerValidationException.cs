namespace MovieAPI.Services.Exceptions
{
    public class ProducerValidationException : Exception
    {
        public ProducerValidationException(string message)
            :base(message)
        {
            
        }
    }
}
