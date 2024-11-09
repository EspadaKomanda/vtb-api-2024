using EntertaimentService.Database.Models;
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
    IRepository<EntertaimentService.Database.Models.Entertaiment> Entertaiments { get; }
    IRepository<EntertaimentService.Database.Models.EntertaimentCategory> EntertaimentCategories { get; }
    IRepository<EntertaimentService.Database.Models.EntertaimentWish> EntertaimentWishes { get; }
    IRepository<EntertaimentService.Database.Models.EntertaimentPaymentMethod> EntertaimentPayments { get; }
    IRepository<PaymentMethod> PaymentMethods { get; }
    IRepository<PaymentVariant> PaymentVariants { get; }
    public int Save();
}
