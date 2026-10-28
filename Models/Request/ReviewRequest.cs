namespace MovieAPI.Models.Request
{
    public class ReviewRequest
    {
        public string Message { get; set; }

        public int MovieId { get; set; }
    }
}
