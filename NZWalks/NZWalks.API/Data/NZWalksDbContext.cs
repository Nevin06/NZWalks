using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
        }

        // use the NZDbContext so that it can query or persist data to the regions table.
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
