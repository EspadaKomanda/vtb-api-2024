using UserService.Database.Models;
using UserService.Repositories;

namespace UserService.Repositories;

public interface IUnitOfWork : IDisposable
{
    ITransaction BeginTransaction();
    IRepository<User> Users { get; }
    IRepository<Meta> Metas { get; }
    IRepository<PersonalData> PersonalDatas { get; }
    IRepository<RegistrationCode> RegistrationCodes { get; }
    IRepository<ResetCode> ResetCodes { get; }
    IRepository<VisitedTour> VisitedTours { get; }
    IRepository<Role> Roles { get; }
}
