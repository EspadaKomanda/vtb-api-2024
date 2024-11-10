using Microsoft.EntityFrameworkCore;
using PromoService.Database.Models;

namespace PromoService.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Promo> Promos { get; set; } = null!;
    public DbSet<PromoCategory> PromoCategories { get; set; } = null!;
    public DbSet<UserPromo> UserPromos { get; set; } = null!;
    public DbSet<PromoAppliedProduct> PromoAppliedProducts { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}