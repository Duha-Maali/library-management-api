using System.Linq.Expressions;
using LibraryManagement.Data.Helpers;
using LibraryManagement.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly LibraryDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        var entityType = typeof(TEntity).Name;
        var keyName = $"{entityType}Id"; // e.g., BookId, BorrowId
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, keyName) == id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        
    }
}
