namespace MovieAPI.Models.Response
{
    public class ActorResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }
    }
}
