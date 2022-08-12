using BlazorGG_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazorGG_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
