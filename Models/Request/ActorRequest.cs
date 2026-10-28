namespace MovieAPI.Models.Request
{
    public class ActorRequest
    {
        public string Name { get; set; }

        public string Biography { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }
    }
}
