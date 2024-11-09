using Microsoft.EntityFrameworkCore;
using EntertaimentService.Database.Models;

namespace EntertaimentService.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Benefit> Benefits { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<ReviewBenefit> ReviewBenefits { get; set; } = null!;
        public DbSet<ReviewFeedback> ReviewFeedbacks { get; set; } = null!;
        public DbSet<EntertaimentService.Database.Models.Entertaiment> Entertaiments { get; set; } = null!;
        public DbSet<EntertaimentService.Database.Models.EntertaimentCategory> EntertaimentCategories { get; set; } = null!;
        public DbSet<EntertaimentService.Database.Models.EntertaimentWish> EntertaimentWishes { get; set; } = null!;
        public DbSet<EntertaimentService.Database.Models.EntertaimentPaymentMethod> EntertaimentPaymentMethods { get; set;} = null!;
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