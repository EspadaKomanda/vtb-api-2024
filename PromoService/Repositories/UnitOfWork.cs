using Microsoft.EntityFrameworkCore;
using PromoService.Database;
using PromoService.Database.Models;

namespace PromoService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IRepository<Promo> PromoRepo { get; }
    public IRepository<PromoCategory> PromoCategoryRepo { get; }
    public IRepository<UserPromo> UserPromoRepo { get; }
    public IRepository<PromoAppliedProduct> PromoAppliedProductRepo { get; }
    private readonly ILogger<UnitOfWork> _logger;
    private readonly ApplicationContext _context;

    public UnitOfWork(IRepository<Promo> promoRepository, IRepository<PromoCategory> promoCategoryRepository, IRepository<UserPromo> userPromoRepository, IRepository<PromoAppliedProduct> promoAppliedProductRepository, ILogger<UnitOfWork> logger, ApplicationContext context)
    {
        PromoRepo = promoRepository;
        PromoCategoryRepo = promoCategoryRepository;
        UserPromoRepo = userPromoRepository;
        PromoAppliedProductRepo = promoAppliedProductRepository;
        
        _logger = logger;
        _context = context;
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