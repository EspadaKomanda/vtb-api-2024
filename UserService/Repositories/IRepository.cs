using System.Linq.Expressions;


namespace UserService.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll(FindOptions? findOptions = null);
    TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null);
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null);
    bool Add(TEntity entity);
    bool AddMany(IEnumerable<TEntity> entities);
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
    bool DeleteMany(Expression<Func<TEntity, bool>> predicate);
    bool Any(Expression<Func<TEntity, bool>> predicate);
    int Count(Expression<Func<TEntity, bool>> predicate);
}