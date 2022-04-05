using Currencies.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Currencies.Data.Context
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                    //Configuration.GetConnectionString("DefaultConnection"),
                    "server=.; database=Currencies;Trusted_Connection=True;",
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(60).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<>
        }
    }
}
