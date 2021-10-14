using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MapWebApi.Models
{
    public class SubdivisionsContext:DbContext
    {
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Commander> Commanders { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }
        public DbSet<TypeOfSubdivision> TypesOfSubdivision { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ActualData> ActualData { get; set; }
        public SubdivisionsContext(DbContextOptions<SubdivisionsContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(message=>Debug.WriteLine(message));
        }
    }
}
