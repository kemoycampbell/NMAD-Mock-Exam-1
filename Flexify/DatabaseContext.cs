using Flexify.Models;
using Microsoft.EntityFrameworkCore;

namespace Flexify
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        :base(options)
        {
            
        }
        
        public DbSet<Film> Films { get; set; }
        public DbSet<Auth> Auths { get; set; }
    }
}