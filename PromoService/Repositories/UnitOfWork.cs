using Microsoft.EntityFrameworkCore;
using PromoService.Database;

namespace PromoService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly ApplicationContext _context;

    public UnitOfWork(ILogger<UnitOfWork> logger, ApplicationContext context)
    {
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