using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{

    // make cityinfocontext derive from the Database context - bigger application often use mutiple contexts
    public class CityinfoContext : DbContext
    {
        public DbSet<City> cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterest { get; set; }

        public CityinfoContext(DbContextOptions<CityinfoContext> options)
            :base(options)
        {
        }

        //// Configure Dbcontext - by overidding
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}
