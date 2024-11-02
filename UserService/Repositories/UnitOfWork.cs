using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Database.Models;

namespace UserService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Meta> Metas { get; }
    public IRepository<VisitedTour> VisitedTours { get; }
    public IRepository<RegistrationCode> RegistrationCodes { get; }
    public IRepository<ResetCode> ResetCodes { get; }
    public IRepository<PersonalData> PersonalDatas { get; }

    public UnitOfWork(ApplicationContext context, IRepository<User> users, IRepository<Role> roles, IRepository<Meta> metas,
        IRepository<VisitedTour> visitedTours, IRepository<RegistrationCode> registrationCodes, IRepository<ResetCode> resetCodes,
        IRepository<PersonalData> personalDatas)
    {
        _context = context;
        Users = users;
        Roles = roles;
        Metas = metas;
        VisitedTours = visitedTours;
        RegistrationCodes = registrationCodes;
        ResetCodes = resetCodes;
        PersonalDatas = personalDatas;
    }

    public int Save()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public ITransaction BeginTransaction()
    {
        return new Transaction(_context);
    }
}