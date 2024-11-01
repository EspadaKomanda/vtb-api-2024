using System.Linq.Expressions;
using UserService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UserService.Repository;

public class Repository<TEntity>(ApplicationContext empDBContext) : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationContext _empDBContext = empDBContext;
    private IDbContextTransaction? _transaction;

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _empDBContext.Set<TEntity>().AddAsync(entity);
        return await _empDBContext.SaveChangesAsync()>= 0;
    }
    public async Task<bool> AddManyAsync(IEnumerable<TEntity> entities)
    {
        await _empDBContext.Set<TEntity>().AddRangeAsync(entities);
        return await _empDBContext.SaveChangesAsync()>= 0;
    }
    public bool Delete(TEntity entity)
    {
        _empDBContext.Set<TEntity>().Remove(entity);
        return _empDBContext.SaveChanges()>= 0;
    }
    public bool DeleteMany(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = Find(predicate);
        _empDBContext.Set<TEntity>().RemoveRange(entities);
        return _empDBContext.SaveChanges()>= 0;
    }
    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
    {
        return await Get(findOptions).FirstOrDefaultAsync(predicate)! ?? throw new NullReferenceException("Entity not found!");
    }
    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
    {
        return Get(findOptions).Where(predicate);
    }
    public IQueryable<TEntity> GetAll(FindOptions? findOptions = null)
    {
        return Get(findOptions);
    }
    public bool Update(TEntity entity)
    {
        _empDBContext.Set<TEntity>().Update(entity);
        return _empDBContext.SaveChanges()>= 0;
    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _empDBContext.Set<TEntity>().AnyAsync(predicate);
    }
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _empDBContext.Set<TEntity>().CountAsync(predicate);
    }
    private DbSet<TEntity> Get(FindOptions? findOptions = null)
    {
        findOptions ??= new FindOptions();
        var entity = _empDBContext.Set<TEntity>();
        if (findOptions.IsAsNoTracking && findOptions.IsIgnoreAutoIncludes)
        {
            entity.IgnoreAutoIncludes().AsNoTracking();
        }
        else if (findOptions.IsIgnoreAutoIncludes)
        {
            entity.IgnoreAutoIncludes();
        }
        else if (findOptions.IsAsNoTracking)
        {
            entity.AsNoTracking();
        }
        return entity;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _empDBContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }
}