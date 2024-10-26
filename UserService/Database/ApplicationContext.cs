using Microsoft.EntityFrameworkCore;
using UserService.Database.Models;

namespace UserService.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Meta> Metas { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<PersonalData> PersonalDatas { get; set; } = null!;
    public DbSet<VisitedTour> VisitedTours { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role {Id = 1, Name = "user", IsProtected = true},
            new Role {Id = 2, Name = "admin", IsProtected = true}
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

}