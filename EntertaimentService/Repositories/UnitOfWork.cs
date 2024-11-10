using Microsoft.EntityFrameworkCore;
using EntertaimentService.Database;
using EntertaimentService.Database.Models;

namespace UserService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly ApplicationContext _context;

    public IRepository<Benefit> Benefits { get; }

    public IRepository<Category> Categories { get; }

    public IRepository<Photo> Photos { get; }

    public IRepository<Review> Reviews { get; }

    public IRepository<ReviewFeedback> ReviewFeedbacks { get; }
    
    public IRepository<ReviewBenefit> ReviewBenefits { get; }

    public IRepository<EntertaimentService.Database.Models.Entertaiment> Entertaiments { get; }

    public IRepository<EntertaimentCategory> EntertaimentCategories { get; }

    public IRepository<EntertaimentWish> EntertaimentWishes { get; }

    public IRepository<EntertaimentPaymentMethod> EntertaimentPayments { get; }

    public IRepository<PaymentMethod> PaymentMethods { get; }

    public IRepository<PaymentVariant> PaymentVariants { get; }

    public UnitOfWork(
        ILogger<UnitOfWork> logger, 
        ApplicationContext context,
        IRepository<Benefit> benefits,
        IRepository<Category> categories,
        IRepository<Photo> photos,
        IRepository<Review> reviews,
        IRepository<ReviewFeedback> reviewFeedbacks,
        IRepository<ReviewBenefit> reviewBenefits,
        IRepository<EntertaimentService.Database.Models.Entertaiment> entertaiments,
        IRepository<EntertaimentCategory> entertaimentCategories,
        IRepository<EntertaimentWish> entertaimentWishes,
        IRepository<EntertaimentPaymentMethod> entertaimentPayments ,
        IRepository<PaymentMethod> paymentMethods,
        IRepository<PaymentVariant> paymentVariants)
    {
        _logger = logger;
        _context = context;
        Benefits = benefits;
        Categories = categories;
        Photos = photos;
        Reviews = reviews;
        ReviewFeedbacks = reviewFeedbacks;
        ReviewBenefits = reviewBenefits;
        Entertaiments = entertaiments;
        EntertaimentCategories = entertaimentCategories;
        EntertaimentWishes = entertaimentWishes;
        EntertaimentPayments = entertaimentPayments;
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