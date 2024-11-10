using Microsoft.EntityFrameworkCore;
using TourService.Database.Models;

namespace TourService.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Benefit> Benefits { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<ReviewBenefit> ReviewBenefits { get; set; } = null!;
        public DbSet<ReviewFeedback> ReviewFeedbacks { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Tour> Tours { get; set; } = null!;
        public DbSet<TourCategory> TourCategories { get; set; } = null!;
        public DbSet<TourWish> TourWishes { get; set; } = null!;
        public DbSet<TourPaymentMethod> TourPaymentMethods { get; set;} = null!;
        public DbSet<TourTag> TourTags { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public DbSet<PaymentVariant> PaymentVariants { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}