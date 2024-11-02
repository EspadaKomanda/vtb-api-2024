using System.Linq.Expressions;
using UserService.Database;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationContext _empDBContext;

    public Repository(ApplicationContext empDBContext)
    {
        _empDBContext = empDBContext;
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _empDBContext.Set<TEntity>().AddAsync(entity);
        return true;
    }
    public async Task<bool> AddManyAsync(IEnumerable<TEntity> entities)
    {
        await _empDBContext.Set<TEntity>().AddRangeAsync(entities);
        return true;
    }
    public bool Delete(TEntity entity)
    {
        _empDBContext.Set<TEntity>().Remove(entity);
        return true;
    }
    public bool DeleteMany(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = Find(predicate);
        _empDBContext.Set<TEntity>().RemoveRange(entities);
        return true;
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
        return true;
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
}