using LibraryManagement.Data.Helpers;
using LibraryManagement.Data.IRepositories;
using LibraryManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly LibraryDbContext _context;

    public UserRepository(LibraryDbContext context) : base(context)
    {
        _context = context;
    }

    //public async Task<User?> GetByUsernameAsync(string username)
    //{
    //    return await _context.Users
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(a => a.UserName == username);
    //}
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role) // Include Role navigation property
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.UserName == username);
    }
}
