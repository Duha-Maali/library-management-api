using LibraryManagement.Business.ViewModels;

namespace LibraryManagement.Business.IServices;

public interface IBorrowerService
{
    Task<IEnumerable<BorrowerVM>> GetAllAsync();
    Task<BorrowerVM?> GetByIdAsync(int id);
    Task<BorrowerVM> CreateAsync(BorrowerVM borrower);
    Task<BorrowerVM> UpdateAsync(int id, BorrowerVM borrower);
    Task<bool> DeleteAsync(int id);
}
