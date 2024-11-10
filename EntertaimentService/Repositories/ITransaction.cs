namespace UserService.Repositories;

public interface ITransaction : IDisposable
{
    void Commit();
    bool SaveAndCommit();
    void Rollback();
}