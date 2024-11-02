using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Database.Models;

namespace UserService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly ApplicationContext _context;
    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Meta> Metas { get; }
    public IRepository<VisitedTour> VisitedTours { get; }
    public IRepository<RegistrationCode> RegistrationCodes { get; }
    public IRepository<ResetCode> ResetCodes { get; }
    public IRepository<PersonalData> PersonalDatas { get; }

    public UnitOfWork(ILogger<UnitOfWork> logger, ApplicationContext context, IRepository<User> users, IRepository<Role> roles, IRepository<Meta> metas,
        IRepository<VisitedTour> visitedTours, IRepository<RegistrationCode> registrationCodes, IRepository<ResetCode> resetCodes,
        IRepository<PersonalData> personalDatas)
    {
        _logger = logger;
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