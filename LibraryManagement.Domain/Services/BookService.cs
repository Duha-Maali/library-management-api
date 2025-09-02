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

public class BookService : IBookService
{
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookService(
        IGenericRepository<Book> bookRepository,
        IGenericRepository<Category> categoryRepository,
        IMapper mapper,
        ILogger<BookService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<BookVM>?> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all books");
            var books = await _bookRepository.GetAllAsync(b => b.Category);
            if (books == null || !books.Any())
            {
                throw new EntityNotFoundException("Books", 0);
            }
            _logger.LogInformation("Retrieved {Count} books", books.Count());
            return _mapper.Map<IEnumerable<BookVM>>(books);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error retrieving books: {Message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving books");
            return null;
        }
    }

    public async Task<BookVM?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving book with BookId: {BookId}", id);
            var book = await _bookRepository.GetByIdAsync(id, b => b.Category);
            if (book == null)
            {
                throw new EntityNotFoundException("Book", id);
            }
            _logger.LogInformation("Retrieved book with BookId: {BookId}, Title: {Title}", book.BookId, book.Title);
            return _mapper.Map<BookVM>(book);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error retrieving book with ID {Id}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving book with ID {Id}", id);
            return null;
        }
    }

    public async Task<BookVM?> CreateAsync(CreateBookVM bookVM)
    {
        try
        {
            _logger.LogInformation("Creating book with Title: {Title}", bookVM.Title);
            var category = await _categoryRepository.GetByIdAsync(bookVM.CategoryId);
            if (category == null)
            {
                throw new InvalidCategoryException(bookVM.CategoryId);
            }
            var book = _mapper.Map<Book>(bookVM);
            book.UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
            await _bookRepository.AddAsync(book);
            _logger.LogInformation("Book created with ID {Id}", book.BookId);
            var createdBook = await _bookRepository.GetByIdAsync(book.BookId, b => b.Category);
            return _mapper.Map<BookVM>(createdBook);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error creating book with Title: {Title}: {Message}", bookVM.Title, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating book with Title: {Title}", bookVM.Title);
            return null;
        }
    }

    public async Task<BookVM?> UpdateAsync(int id, CreateBookVM bookVM)
    {
        try
        {
            _logger.LogInformation("Updating book with ID: {Id}", id);
            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
            {
                throw new EntityNotFoundException("Book", id);
            }
            var category = await _categoryRepository.GetByIdAsync(bookVM.CategoryId);
            if (category == null)
            {
                throw new InvalidCategoryException(bookVM.CategoryId);
            }
            int availableCopies = existingBook.TotalCopies - existingBook.BorrowedCopies;
            if (bookVM.TotalCopies < availableCopies)
            {
                throw new InvalidBookCopyCountException();
            }
            _mapper.Map(bookVM, existingBook);
            await _bookRepository.UpdateAsync(existingBook);
            _logger.LogInformation("Updated book with ID: {Id}, Title: {Title}", id, existingBook.Title);
            existingBook = await _bookRepository.GetByIdAsync(id, b => b.Category);
            return _mapper.Map<BookVM>(existingBook);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error updating book with ID {Id}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating book with ID {Id}", id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting book with BookId: {BookId}", id);
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new EntityNotFoundException("Book", id);
            }
            await _bookRepository.DeleteAsync(book);
            _logger.LogInformation("Deleted book with BookId: {BookId}", id);
            return true;
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error deleting book with BookId: {BookId}: {Message}", id, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting book with BookId: {BookId}", id);
            return false;
        }
    }
}