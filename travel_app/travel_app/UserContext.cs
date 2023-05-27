using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app
{
    public class UserContext : DbContext
    {
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=myDb;trusted_connection=true;");
        }

        public async Task AddUserAsync(User user)
        {
            // Check if the user is null or already exists
            if (user == null || User.Any(u => u.Username == user.Username))
            {
                throw new ArgumentException("Invalid user");
            }

            // Add the user to the User DbSet
            User.Add(user);

            // Save the changes to the database
            await SaveChangesAsync();
        }
    }
}
