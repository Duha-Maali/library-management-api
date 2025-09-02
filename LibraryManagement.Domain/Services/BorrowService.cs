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

    public async Task<IEnumerable<BorrowVM>?> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all borrows");
            var borrows = await _borrowRepository.GetAllAsync(b => b.Borrower, b => b.Book);
            _logger.LogInformation("Retrieved {Count} borrows", borrows.Count());
            return _mapper.Map<IEnumerable<BorrowVM>>(borrows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving borrows");
            return null;
        }
    }

    public async Task<BorrowVM?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving borrow with BorrowId: {BorrowId}", id);
            var borrow = await _borrowRepository.GetByIdAsync(id, b => b.Borrower, b => b.Book);
            if (borrow == null)
            {
                throw new EntityNotFoundException("Borrow", id);
            }
            _logger.LogInformation("Retrieved borrow with BorrowId: {BorrowId}", borrow.BorrowId);
            return _mapper.Map<BorrowVM>(borrow);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error retrieving borrow with BorrowId {BorrowId}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving borrow with BorrowId {BorrowId}", id);
            return null;
        }
    }

    public async Task<BorrowVM?> CreateAsync(CreateBorrowVM borrowVM)
    {
        try
        {
            _logger.LogInformation("Creating borrow with BorrowerId: {BorrowerId}, BookId: {BookId}",
                borrowVM.BorrowerId, borrowVM.BookId);

            var borrower = await _borrowerRepository.GetByIdAsync(borrowVM.BorrowerId);
            if (borrower == null)
            {
                throw new EntityNotFoundException("Borrower", borrowVM.BorrowerId);
            }

            var book = await _bookRepository.GetByIdAsync(borrowVM.BookId);
            if (book == null)
            {
                throw new EntityNotFoundException("Book", borrowVM.BookId);
            }
            int availableCopies = book.TotalCopies - book.BorrowedCopies;
            if (availableCopies <= 0)
            {
                throw new NoAvailableCopiesException(borrowVM.BookId);
            }

            if (borrowVM.DueDate <= borrowVM.BorrowDate)
            {
                throw new InvalidBorrowDateException();
            }

            book.BorrowedCopies++;
            await _bookRepository.UpdateAsync(book);

            var borrow = _mapper.Map<Borrow>(borrowVM);
            borrow.UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
            await _borrowRepository.AddAsync(borrow);
            _logger.LogInformation("Created borrow with BorrowId: {BorrowId}", borrow.BorrowId);
            var createdBorrow = await _borrowRepository.GetByIdAsync(borrow.BorrowId, b => b.Borrower, b => b.Book);
            return _mapper.Map<BorrowVM>(createdBorrow);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error creating borrow with BorrowerId: {BorrowerId}, BookId: {BookId}: {Message}",
                borrowVM.BorrowerId, borrowVM.BookId, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating borrow with BorrowerId: {BorrowerId}, BookId: {BookId}",
                borrowVM.BorrowerId, borrowVM.BookId);
            return null;
        }
    }

    public async Task<BorrowVM?> UpdateAsync(int id, UpdateBorrowVM borrowVM)
    {
        try
        {
            _logger.LogInformation("Updating borrow with BorrowId: {BorrowId}", id);
            var existingBorrow = await _borrowRepository.GetByIdAsync(id);
            if (existingBorrow == null)
            {
                throw new EntityNotFoundException("Borrow", id);
            }

            if (borrowVM.ReturnDate < existingBorrow.BorrowDate)
            {
                throw new InvalidReturnDateException();
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

            book.BorrowedCopies--;
            await _bookRepository.UpdateAsync(book);

            _mapper.Map(borrowVM, existingBorrow);
            await _borrowRepository.UpdateAsync(existingBorrow);
            _logger.LogInformation("Updated borrow with BorrowId: {BorrowId}", id);
            var updatedBorrow = await _borrowRepository.GetByIdAsync(id, b => b.Borrower, b => b.Book);
            return _mapper.Map<BorrowVM>(updatedBorrow);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error updating borrow with BorrowId {BorrowId}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating borrow with BorrowId {BorrowId}", id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting borrow with BorrowId: {BorrowId}", id);
            var borrow = await _borrowRepository.GetByIdAsync(id);
            if (borrow == null)
            {
                throw new EntityNotFoundException("Borrow", id);
            }

            if (borrow.Status != BorrowStatus.Returned)
            {
                throw new BorrowNotReturnedException(id);
            }

            borrow.Status = BorrowStatus.Inactive;
            await _borrowRepository.UpdateAsync(borrow);
            _logger.LogInformation("Deleted borrow with BorrowId: {BorrowId}", id);
            return true;
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error deleting borrow with BorrowId {BorrowId}: {Message}", id, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting borrow with BorrowId {BorrowId}", id);
            return false;
        }
    }
}