using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.Models;

namespace LibraryManagement.Business.IServices;

public interface IBookService
{
    Task<IEnumerable<BookVM>?> GetAllAsync();
    Task<BookVM?> GetByIdAsync(int id);
    Task<BookVM> CreateAsync(BookVM book);
    Task<BookVM> UpdateAsync(int id, BookVM book);
    Task<bool> DeleteAsync(int id);
}
