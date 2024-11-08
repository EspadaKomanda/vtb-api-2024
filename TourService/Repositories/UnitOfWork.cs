using Microsoft.EntityFrameworkCore;
using TourService.Database;
using TourService.Database.Models;

namespace UserService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly ApplicationContext _context;

    public IRepository<Benefit> Benefits { get; }

    public IRepository<Category> Categories { get; }

    public IRepository<Photo> Photos { get; }

    public IRepository<Review> Reviews { get; }

    public IRepository<ReviewBenefit> ReviewBenefits { get; }

    public IRepository<Tag> Tags { get; }

    public IRepository<Tour> Tours { get; }

    public IRepository<TourCategory> TourCategories { get; }

    public IRepository<TourTag> TourTags { get;}

    public IRepository<TourWish> TourWishes{ get; }

    public IRepository<TourPaymentMethod> TourPayments { get; }

    public IRepository<PaymentMethod> PaymentMethods { get; }

    public IRepository<PaymentVariant> PaymentVariants { get; }

    public UnitOfWork(
        ILogger<UnitOfWork> logger, 
        ApplicationContext context,
        IRepository<Benefit> benefits,
        IRepository<Category> categories,
        IRepository<Photo> photos,
        IRepository<Review> reviews,
        IRepository<ReviewBenefit> reviewBenefits,
        IRepository<Tag> tags,
        IRepository<Tour> tours,
        IRepository<TourCategory> tourCategories,
        IRepository<TourTag> tourTags,
        IRepository<TourWish> tourWishes,
        IRepository<TourPaymentMethod> tourPayments,
        IRepository<PaymentMethod> paymentMethods,
        IRepository<PaymentVariant> paymentVariants)
    {
        _logger = logger;
        _context = context;
        Benefits = benefits;
        Categories = categories;
        Photos = photos;
        Reviews = reviews;
        ReviewBenefits = reviewBenefits;
        Tags = tags;
        Tours = tours;
        TourCategories = tourCategories;
        TourWishes = tourWishes;
        TourPayments = tourPayments;
        TourTags = tourTags;
        PaymentMethods = paymentMethods;
        PaymentVariants = paymentVariants;
        
    }

    public int Save()
    {
        return _context.SaveChanges();
    }

    [Obsolete("UnitOfWork is not supposed to be manually disposed.")]
    public void Dispose()
    {
        _logger.LogWarning("UnitOfWork is not supposed to be manually disposed. Causing this method may cause trouble!");
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public ITransaction BeginTransaction()
    {
        return new Transaction(_context);
    }
}