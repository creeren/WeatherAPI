using Microsoft.EntityFrameworkCore;

namespace WeatherAPI.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Models.Weather> WeatherRecords { get; set; }
    }
}
