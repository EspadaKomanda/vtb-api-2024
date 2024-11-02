namespace UserService.Repositories;

public interface ITransaction : IDisposable
{
    void Commit();

    void Rollback();
}