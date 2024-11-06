using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PromoService.Repositories;

public class Transaction(DbContext context) : ITransaction
{
    private readonly DbContext _context = context;
    private readonly IDbContextTransaction _transaction = context.Database.BeginTransaction();

    public void Commit()
    {
        _transaction.Commit();
    }

    public bool SaveAndCommit()
    {
        var result = _context.SaveChanges() >= 0;
        _transaction.Commit();
        return result;
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }

    public void Dispose()
    {
        _transaction.Dispose();
        GC.SuppressFinalize(this);
    }
}