using PromoService.Database.Models;
using PromoService.Repositories;

namespace PromoService.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IRepository<Promo> PromoRepo { get; }
    public  IRepository<PromoCategory> PromoCategoryRepo { get; }
    public  IRepository<UserPromo> UserPromoRepo { get; }
    public  IRepository<PromoAppliedProduct> PromoAppliedProductRepo { get; }
    
    ITransaction BeginTransaction();

    public int Save();
}