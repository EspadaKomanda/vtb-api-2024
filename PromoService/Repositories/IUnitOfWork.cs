using PromoService.Repositories;

namespace PromoService.Repositories;

public interface IUnitOfWork : IDisposable
{
    ITransaction BeginTransaction();

    public int Save();
}