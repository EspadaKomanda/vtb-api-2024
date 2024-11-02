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

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;

        Users = new Repository<User>(_context);
        Roles = new Repository<Role>(_context);
        Metas = new Repository<Meta>(_context);
        VisitedTours = new Repository<VisitedTour>(_context);
        RegistrationCodes = new Repository<RegistrationCode>(_context);
        ResetCodes = new Repository<ResetCode>(_context);
        PersonalDatas = new Repository<PersonalData>(_context);
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