using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourService.Database.Models;

namespace TourService.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EntartaimentWish> EntartaimentWishes { get; set; }
        public DbSet<Entertaiment> Entertaiments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewBenefit> ReviewBenefits { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourCategory> TourCategories { get; set; }
        public DbSet<TourWish> TourWishes { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}