using TourService.Database.Models;
using UserService.Repositories;

namespace UserService.Repositories;

public interface IUnitOfWork : IDisposable
{
    ITransaction BeginTransaction();
    IRepository<Benefit> Benefits { get; }
    IRepository<Category> Categories { get; }
    IRepository<Photo> Photos { get; }
    IRepository<Review> Reviews { get; }
    IRepository<ReviewBenefit> ReviewBenefits { get; }
    IRepository<ReviewFeedback> ReviewFeedbacks { get; }
    IRepository<Tag> Tags { get; }
    IRepository<Tour> Tours { get; }
    IRepository<TourCategory> TourCategories { get; }
    IRepository<TourTag> TourTags { get; }
    IRepository<TourWish> TourWishes { get; }
    IRepository<TourPaymentMethod> TourPayments { get; }
    IRepository<PaymentMethod> PaymentMethods { get; }
    IRepository<PaymentVariant> PaymentVariants { get; }
    public int Save();
}
