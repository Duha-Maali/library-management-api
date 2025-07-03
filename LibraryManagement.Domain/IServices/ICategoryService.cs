using LibraryManagement.Business.ViewModels;

namespace LibraryManagement.Business.IServices;

public interface ICategoryService
{
    Task<IEnumerable<CategoryVM>> GetAllAsync();
    Task<CategoryVM?> GetByIdAsync(int id);
    Task<CategoryVM> CreateAsync(CategoryVM category);
    Task<CategoryVM> UpdateAsync(int id, CategoryVM category);
    Task<bool> DeleteAsync(int id);
}
