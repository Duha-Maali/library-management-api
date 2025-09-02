using LibraryManagement.Business.Exceptions;
using System.Security.Claims;
using AutoMapper;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.IRepositories;
using LibraryManagement.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CategoryService(
        IGenericRepository<Category> categoryRepository,
        IGenericRepository<Book> bookRepository,
        IMapper mapper,
        ILogger<CategoryService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _categoryRepository = categoryRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<CategoryVM>?> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all categories");
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                throw new EntityNotFoundException("Categories", 0);
            }
            _logger.LogInformation("Retrieved {Count} categories", categories.Count());
            return _mapper.Map<IEnumerable<CategoryVM>>(categories);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error retrieving categories: {Message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving categories");
            return null;
        }
    }

    public async Task<CategoryVM?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving category with ID {Id}", id);
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new EntityNotFoundException("Category", id);
            }
            _logger.LogInformation("Retrieved category with ID {Id}, Name: {Name}", id, category.CategoryName);
            return _mapper.Map<CategoryVM>(category);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error retrieving category with ID {Id}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving category with ID {Id}", id);
            return null;
        }
    }

    public async Task<CategoryVM?> CreateAsync(CreateCategoryVM categoryVM)
    {
        try
        {
            _logger.LogInformation("Creating new category with Name: {Name}", categoryVM.CategoryName);
            var existingCategories = await _categoryRepository.GetAllAsync();
            if (existingCategories.Any(c => c.CategoryName.Equals(categoryVM.CategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new CategoryNameDuplicateException(categoryVM.CategoryName);
            }
            var category = _mapper.Map<Category>(categoryVM);
            category.UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
            await _categoryRepository.AddAsync(category);
            _logger.LogInformation("Category created with ID {Id}", category.CategoryId);
            return _mapper.Map<CategoryVM>(category);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error creating category with Name: {Name}: {Message}", categoryVM.CategoryName, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating category with Name: {Name}", categoryVM.CategoryName);
            return null;
        }
    }

    public async Task<CategoryVM?> UpdateAsync(int id, CreateCategoryVM categoryVM)
    {
        try
        {
            _logger.LogInformation("Updating category with ID {Id}", id);
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new EntityNotFoundException("Category", id);
            }
            var otherCategories = await _categoryRepository.GetAllAsync();
            if (otherCategories.Any(c => c.CategoryName.Equals(categoryVM.CategoryName, StringComparison.OrdinalIgnoreCase) && c.CategoryId != id))
            {
                throw new CategoryNameDuplicateException(categoryVM.CategoryName);
            }
            _mapper.Map(categoryVM, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);
            return _mapper.Map<CategoryVM>(existingCategory);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error updating category with ID {Id}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating category with ID {Id}", id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting category with ID {Id}", id);
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new EntityNotFoundException("Category", id);
            }

            var books = await _bookRepository.GetAllAsync();
            if (books.Any(b => b.CategoryId == id))
            {
                throw new CategoryInUseException(id);
            }
            await _categoryRepository.DeleteAsync(category);
            return true;
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error deleting category with ID {Id}: {Message}", id, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting category with ID {Id}", id);
            return false;
        }
    }
}