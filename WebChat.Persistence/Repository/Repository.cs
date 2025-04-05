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

    public async Task<PagedList<T>> GetAll(Expression<Func<T, bool>>? filter = null, PaginationParameters? pagParams = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        // default pagination parameters if pagParams is null
        pagParams ??= new PaginationParameters();

        if(filter != null)
        {
            query = query.Where(filter);
        }

        if(orderBy != null)
        {
            query = orderBy(query);
        }

        var totalItems = await query.CountAsync();
         
        query = query
            .Skip((pagParams.Page - 1) * pagParams.PageSize)
            .Take(pagParams.PageSize);


        if (!string.IsNullOrEmpty(includeProperties))
        {
            query = IncludeProperties(query, includeProperties);
        }

        var list = await query.ToListAsync();
        return PagedList<T>.ToPagedList(list,pagParams.Page,pagParams.PageSize,totalItems);
    }

    public async Task<T> GetByFilter(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
        {
            query = IncludeProperties(query, includeProperties);
        }

        return await query.FirstOrDefaultAsync();
    }


    public async Task Update(T entity)
    {
        dbSet.Update(entity);
        await _db.SaveChangesAsync();
    }
    
    private IQueryable<T> IncludeProperties(IQueryable<T> query, string includeProperties)
    {
        foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProp);
        }

        return query;
    }
}