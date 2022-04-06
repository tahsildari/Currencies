using Currencies.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Currencies.Data.SeedData
{
    public static class ModelBuilderSeedDataExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, FirstName = "Mahdi", LastName = "Tahsildari", Password = "1234", Username = "mt" },
                new User() { Id = 2, FirstName = "Shaun", LastName = "Grech", Password = "1234", Username = "sg" });
        }
    }
}
