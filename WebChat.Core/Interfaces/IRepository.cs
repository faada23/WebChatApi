using System.Linq.Expressions;

public interface IRepository<T> where T : class 
{
    Task Insert(T entity);
    Task Delete(Guid id);
    Task Update(T entity);
    Task<T> GetByFilter(Expression<Func<T, bool>> filter, string? includeProperties = null);
    Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null);
}