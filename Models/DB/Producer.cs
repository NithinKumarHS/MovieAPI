namespace MovieAPI.Models.DB
{
    public class Producer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public int GenderId { get; set; }
    }
}
