namespace APIRestSharp.Models
{
    public class UserCreationResponse
    {
        public string Name { get; set; }        // The name of the user.
        public string Job { get; set; }         // The job of the user.
        public string Id { get; set; }          // The ID of the user (as a string).
        public DateTime CreatedAt { get; set; } // The creation timestamp of the user.
    }
}
