using LibraryManagement.Business.ViewModels;

namespace LibraryManagement.Business.IServices;

public interface IBorrowerService
{
    Task<IEnumerable<BorrowerVM>?> GetAllAsync();
    Task<BorrowerVM?> GetByIdAsync(int id);
    Task<BorrowerVM?> CreateAsync(CreateBorrowerVM borrower);
    Task<BorrowerVM?> UpdateAsync(int id, CreateBorrowerVM borrower);
    Task<bool> DeleteAsync(int id);
}
