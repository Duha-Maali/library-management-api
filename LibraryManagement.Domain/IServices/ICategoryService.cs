using LibraryManagement.Business.ViewModels;

namespace LibraryManagement.Business.IServices;

public interface ICategoryService
{
    Task<IEnumerable<CategoryVM>?> GetAllAsync();
    Task<CategoryVM?> GetByIdAsync(int id);
    Task<CategoryVM?> CreateAsync(CreateCategoryVM category);
    Task<CategoryVM?> UpdateAsync(int id, CreateCategoryVM category);
    Task<bool> DeleteAsync(int id);
}
