using AutoMapper;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.IRepositories;
using LibraryManagement.Data.Models;

namespace LibraryManagement.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        IGenericRepository<Category> categoryRepository,
        IGenericRepository<Book> bookRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryVM>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryVM>>(categories);
    }

    public async Task<CategoryVM?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) 
            return null;
        return _mapper.Map<CategoryVM>(category);
    }

    public async Task<CategoryVM> CreateAsync(CategoryVM categoryVM)
    {
        // Validate CategoryName uniqueness
        var existingCategories = await _categoryRepository.GetAllAsync();
        if (existingCategories.Any(c => c.CategoryName.Equals(categoryVM.CategoryName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A category with this name already exists.");

        var category = _mapper.Map<Category>(categoryVM);
        await _categoryRepository.AddAsync(category);
        return _mapper.Map<CategoryVM>(category);
    }

    public async Task<CategoryVM> UpdateAsync(int id, CategoryVM categoryVM)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
            throw new KeyNotFoundException("Category not found.");

        // Validate CategoryName uniqueness (excluding current category)
        var otherCategories = await _categoryRepository.GetAllAsync();
        if (otherCategories.Any(c => c.CategoryName.Equals(categoryVM.CategoryName, StringComparison.OrdinalIgnoreCase) && c.CategoryId != id))
            throw new InvalidOperationException("A category with this name already exists.");

        _mapper.Map(categoryVM, existingCategory);
        await _categoryRepository.UpdateAsync(existingCategory);
        return _mapper.Map<CategoryVM>(existingCategory);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) 
            return false;

        // Check for books using this category
        var books = await _bookRepository.GetAllAsync();
        if (books.Any(b => b.CategoryId == id))
            throw new InvalidOperationException("Cannot delete category used by existing books.");

        await _categoryRepository.DeleteAsync(category);
        return true;
    }
}
