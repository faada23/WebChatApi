using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
    public DatabaseContext _db {get; }
    public DbSet<T> dbSet;

    public Repository(DatabaseContext db){
        _db = db;
        dbSet = _db.Set<T>();
    }

    public async Task Insert(T entity)
    {
        dbSet.Add(entity);
        await _db.SaveChangesAsync();

    }

    public async Task Delete(Guid id)
    {   
        T? entity = dbSet.Find(id);

        if(entity != null){

            dbSet.Remove(entity);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        
        if(filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        if(orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public async Task<T> GetByFilter(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);

        if(!string.IsNullOrEmpty(includeProperties)){
            foreach(var includeProp in includeProperties
            .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries)){
                query = query.Include(includeProp);
            }
        }
        return await query.FirstOrDefaultAsync();
    }


    public async Task Update(T entity)
    {
        dbSet.Update(entity);
        await _db.SaveChangesAsync();
    }
    
}