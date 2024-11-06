using Microsoft.EntityFrameworkCore;
using PromoService.Database;
using PromoService.Database.Models;

namespace PromoService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IRepository<Promo> _promoRepository;
    private readonly IRepository<PromoCategory> _promoCategoryRepository;
    private readonly IRepository<UserPromo> _userPromoRepository;
    private readonly IRepository<PromoAppliedProduct> _promoAppliedProductRepository;
    private readonly ILogger<UnitOfWork> _logger;
    private readonly ApplicationContext _context;

    public UnitOfWork(IRepository<Promo> promoRepository, IRepository<PromoCategory> promoCategoryRepository, IRepository<UserPromo> userPromoRepository, IRepository<PromoAppliedProduct> promoAppliedProductRepository, ILogger<UnitOfWork> logger, ApplicationContext context)
    {
        _promoRepository = promoRepository;
        _promoCategoryRepository = promoCategoryRepository;
        _userPromoRepository = userPromoRepository;
        _promoAppliedProductRepository = promoAppliedProductRepository;
        
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