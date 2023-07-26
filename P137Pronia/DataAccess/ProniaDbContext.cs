using Microsoft.EntityFrameworkCore;
using P137Pronia.Models;

namespace P137Pronia.DataAccess
{
    public class ProniaDbContext:DbContext
    {
        public ProniaDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
    }
}
