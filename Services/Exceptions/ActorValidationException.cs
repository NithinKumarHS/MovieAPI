namespace MovieAPI.Services.Exceptions
{
    public class ActorValidationException : Exception
    {
        public ActorValidationException(string message) 
            : base(message)
        { 
        }
    }
}
