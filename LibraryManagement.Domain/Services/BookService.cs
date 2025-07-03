using AutoMapper;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.IRepositories;
using LibraryManagement.Data.Models;
using LibraryManagement.Data.Repositories;
namespace LibraryManagement.Business.Services;

public class BookService : IBookService
{
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public BookService(IGenericRepository<Book> bookRepository, IGenericRepository<Category> categoryRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookVM>?> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        if (books == null || !books.Any())
            return null;
        return _mapper.Map<IEnumerable<BookVM>>(books);
    }

    public async Task<BookVM?> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) 
            return null;
        return _mapper.Map<BookVM>(book);
    }

    public async Task<BookVM> CreateAsync(BookVM bookVM)
    {
        if (bookVM.AvailableCopies > bookVM.TotalCopies)
            throw new ArgumentException("AvailableCopies cannot exceed TotalCopies.");

        var category = await _categoryRepository.GetByIdAsync(bookVM.CategoryId);

        if(category == null)
            throw new KeyNotFoundException("Invalid Category Id.");

        var book = _mapper.Map<Book>(bookVM);
        await _bookRepository.AddAsync(book);
        return _mapper.Map<BookVM>(book);
    }

    public async Task<BookVM> UpdateAsync(int id, BookVM bookVM)
    {
        if (bookVM.AvailableCopies > bookVM.TotalCopies)
            throw new ArgumentException("AvailableCopies cannot exceed TotalCopies.");

        var category = await _categoryRepository.GetByIdAsync(bookVM.CategoryId);
        if (category == null)
            throw new KeyNotFoundException("Invalid Category Id.");

        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) 
            throw new KeyNotFoundException("Book not found.");

        _mapper.Map(bookVM, book);
        await _bookRepository.UpdateAsync(book);
        return _mapper.Map<BookVM>(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return false;

        await _bookRepository.DeleteAsync(book);
        return true;
    }
}
