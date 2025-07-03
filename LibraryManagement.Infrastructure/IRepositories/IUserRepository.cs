using LibraryManagement.Data.Models;

namespace LibraryManagement.Data.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}
