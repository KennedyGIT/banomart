
using Banomart.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Banomart.Services.ShoppingCartAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<CartHeader> CartHeaders { get; set; }

        public DbSet<CartDetails> CartDetails { get; set; }

    }
}
