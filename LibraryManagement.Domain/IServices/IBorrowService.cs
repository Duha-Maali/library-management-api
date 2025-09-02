using LibraryManagement.Business.ViewModels;

namespace LibraryManagement.Business.IServices;

public interface IBorrowService
{
    Task<IEnumerable<BorrowVM>?> GetAllAsync();
    Task<BorrowVM?> GetByIdAsync(int id);
    Task<BorrowVM?> CreateAsync(CreateBorrowVM borrow);
    Task<BorrowVM?> UpdateAsync(int id, UpdateBorrowVM borrow);
    Task<bool> DeleteAsync(int id);
}
