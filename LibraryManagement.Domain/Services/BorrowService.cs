using System.Security.Claims;
using AutoMapper;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.IRepositories;
using LibraryManagement.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Business.Services;

public class BorrowService : IBorrowService
{
    private readonly IGenericRepository<Borrow> _borrowRepository;
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IGenericRepository<Borrower> _borrowerRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BorrowService> _logger;
    private readonly IMapper _mapper;

    public BorrowService(
        IGenericRepository<Borrow> borrowRepository,
        IGenericRepository<Book> bookRepository,
        IGenericRepository<Borrower> borrowerRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BorrowService> logger,
        IMapper mapper)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _borrowerRepository = borrowerRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _mapper = mapper;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
        return int.Parse(userIdClaim.Value);
    }

    public async Task<IEnumerable<BorrowVM>> GetAllAsync()
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value ?? "Unknown",
            ["RoleName"] = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
        }))
        {
            _logger.LogInformation("Retrieving all borrows");
            var borrows = await _borrowRepository.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} borrows", borrows.Count());
            return _mapper.Map<IEnumerable<BorrowVM>>(borrows);
        }
    }

    public async Task<BorrowVM?> GetByIdAsync(int id)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value ?? "Unknown",
            ["RoleName"] = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
        }))
        {
            _logger.LogInformation("Retrieving borrow with BorrowId: {BorrowId}", id);
            var borrow = await _borrowRepository.GetByIdAsync(id);
            if (borrow == null)
            {
                _logger.LogWarning("Borrow not found with BorrowId: {BorrowId}", id);
                return null;
            }
            _logger.LogInformation("Retrieved borrow with BorrowId: {BorrowId}", borrow.BorrowId);
            return _mapper.Map<BorrowVM>(borrow);
        }
    }

    public async Task<BorrowVM?> CreateAsync(CreateBorrowVM borrowVM)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value ?? "Unknown",
            ["RoleName"] = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
        }))
        {
            _logger.LogInformation("Creating borrow with BorrowerId: {BorrowerId}, BookId: {BookId}",
                    borrowVM.BorrowerId, borrowVM.BookId);

            // Validate Borrower 
            var borrower = await _borrowerRepository.GetByIdAsync(borrowVM.BorrowerId);
            if (borrower == null)
            {
                _logger.LogWarning("Borrower not found with BorrowerId: {borrowVM.BorrowerId}", borrowVM.BorrowerId);
                return null;
            }

            // Validate Book
            var book = await _bookRepository.GetByIdAsync(borrowVM.BookId);
            if (book == null)
            {
                _logger.LogWarning("Book not found with BookId: {borrowVM.BookId}", borrowVM.BookId);
                return null;
            }

            // Validate AvailableCopies
            if (book.AvailableCopies <= 0)
            {
                _logger.LogWarning("No available copies of the book with BookId: {borrowVM.BookId}", borrowVM.BookId);
                return null;
            }

            // Validate Dates
            if (borrowVM.DueDate <= borrowVM.BorrowDate)
            {
                _logger.LogWarning("DueDate must be after BorrowDate.");
                return null;
            }

            // Update AvailableCopies
            book.AvailableCopies--;
            await _bookRepository.UpdateAsync(book);

            var borrow = _mapper.Map<Borrow>(borrowVM);
            borrow.UserId = GetCurrentUserId(); // Assuming UserId is not set during creation
            await _borrowRepository.AddAsync(borrow);
            _logger.LogInformation("Created borrow with BorrowId: {BorrowId}", borrow.BorrowId);
            return _mapper.Map<BorrowVM>(borrow);
        }
    }

    public async Task<BorrowVM?> UpdateAsync(int id, UpdateBorrowVM borrowVM)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value ?? "Unknown",
            ["RoleName"] = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
        }))
        {
            _logger.LogInformation("Updating borrow with BorrowId: {BorrowId}", id);
            var existingBorrow = await _borrowRepository.GetByIdAsync(id);
            if (existingBorrow == null)
            {
                _logger.LogWarning("Borrow not found with BorrowId: {BorrowId}", id);
                return null;
            }

            // validate Return date
            if (borrowVM.ReturnDate < existingBorrow.BorrowDate)
            {
                _logger.LogWarning("ReturnDate cannot be before BorrowDate.");
                return null;
            }

            var book = await _bookRepository.GetByIdAsync(existingBorrow.BookId);

            if (borrowVM.ReturnDate <= existingBorrow.DueDate)
            {
                existingBorrow.Status = BorrowStatus.Returned;
            }
            else
            {
                existingBorrow.Status = BorrowStatus.Overdue;
            }

            book.AvailableCopies++;
            await _bookRepository.UpdateAsync(book);

            _mapper.Map(borrowVM, existingBorrow);
            await _borrowRepository.UpdateAsync(existingBorrow);
            _logger.LogInformation("Updated borrow with BorrowId: {BorrowId}", id);
            return _mapper.Map<BorrowVM>(existingBorrow);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value ?? "Unknown",
            ["RoleName"] = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
        }))
        {
            _logger.LogInformation("Deleting borrow with BorrowId: {BorrowId}", id);
            var borrow = await _borrowRepository.GetByIdAsync(id);
            if (borrow == null)
            {
                _logger.LogWarning("Borrow not found with BorrowId: {BorrowId}", id);
                return false;
            }

            // Prevent deletion of non-returned borrows
            if (borrow.Status != BorrowStatus.Returned)
            {
                _logger.LogWarning("Cannot delete borrow that is not returned.");
                return false;
            }

            borrow.Status = BorrowStatus.Inactive;
            await _borrowRepository.UpdateAsync(borrow);
            _logger.LogInformation("Deleted borrow with BorrowId: {BorrowId}", id);
            return true;
        }      
    }
}
