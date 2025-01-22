using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Task_Management_API.Models
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users
        {
            get; set;
        }
    }
}
