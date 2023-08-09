using TestAPI0807.Models;
using Microsoft.EntityFrameworkCore;

namespace TestAPI0807.Models
{
    public class UserDataContext : DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) 
            : base(options) 
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<UserData> UserDatas { get; set; } = null!;
    }
}
