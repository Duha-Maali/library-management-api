using LibraryManagement.Business.ViewModels;

namespace LibraryManagement.Business.IServices;

public interface IUserService
{
    Task<string?> LoginAsync(LoginVM request);
}
