
using Banomart.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Banomart.Services.EmailAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options){ }

        public DbSet<EmailLog> EmailLogs { get; set; }

        
    }
}
