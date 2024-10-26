using System.Linq.Expressions;
using UserService.Database;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository
{
    public class Repository<TEntity>(ApplicationContext db) : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext _db = db;

        public bool Add(TEntity entity)
        {
            _db.Set<TEntity>().Add(entity);
            return _db.SaveChanges()>= 0;
        }
        public bool AddMany(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().AddRange(entities);
            return _db.SaveChanges()>= 0;
        }
        public bool Delete(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
            return _db.SaveChanges()>= 0;
        }
        public bool DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Find(predicate);
            _db.Set<TEntity>().RemoveRange(entities);
            return _db.SaveChanges()>= 0;
        }
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return Get(findOptions).FirstOrDefault(predicate)!;
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
            _db.Set<TEntity>().Update(entity);
            return _db.SaveChanges()>= 0;
        }
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Any(predicate);
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Count(predicate);
        }
        private DbSet<TEntity> Get(FindOptions? findOptions = null)
        {
            findOptions ??= new FindOptions();
            var entity = _db.Set<TEntity>();
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
}
