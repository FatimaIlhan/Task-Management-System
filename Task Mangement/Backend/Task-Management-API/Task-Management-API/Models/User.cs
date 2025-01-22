namespace Task_Management_API.Models
{
    public class User
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IsActive { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
