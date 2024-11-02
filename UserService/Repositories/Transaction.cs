using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UserService.Repositories;

public class Transaction(DbContext context) : ITransaction
{
    private readonly IDbContextTransaction _transaction = context.Database.BeginTransaction();

    public void Commit()
    {
        _transaction.Commit();
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
