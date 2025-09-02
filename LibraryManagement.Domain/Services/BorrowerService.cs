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

public class BorrowerService : IBorrowerService
{
    private readonly IGenericRepository<Borrower> _borrowerRepository;
    private readonly IGenericRepository<Borrow> _borrowRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BorrowerService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BorrowerService(
        IGenericRepository<Borrower> borrowerRepository,
        IGenericRepository<Borrow> borrowRepository,
        IMapper mapper,
        ILogger<BorrowerService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _borrowerRepository = borrowerRepository;
        _borrowRepository = borrowRepository;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<BorrowerVM>?> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all borrowers");
            var borrowers = await _borrowerRepository.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} borrowers", borrowers.Count());
            return _mapper.Map<IEnumerable<BorrowerVM>>(borrowers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving borrowers");
            return null;
        }
    }

    public async Task<BorrowerVM?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving borrower with ID: {Id}", id);
            var borrower = await _borrowerRepository.GetByIdAsync(id);
            if (borrower == null)
            {
                throw new EntityNotFoundException("Borrower", id);
            }
            return _mapper.Map<BorrowerVM>(borrower);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error retrieving borrower with ID {Id}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving borrower with ID {Id}", id);
            return null;
        }
    }

    public async Task<BorrowerVM?> CreateAsync(CreateBorrowerVM borrowerVM)
    {
        try
        {
            _logger.LogInformation("Creating new borrower: {@BorrowerVM}", borrowerVM);
            var borrower = _mapper.Map<Borrower>(borrowerVM);
            borrower.UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
            await _borrowerRepository.AddAsync(borrower);
            _logger.LogInformation("Borrower created with ID: {Id}", borrower.BorrowerId);
            return _mapper.Map<BorrowerVM>(borrower);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating borrower");
            return null;
        }
    }

    public async Task<BorrowerVM?> UpdateAsync(int id, CreateBorrowerVM borrowerVM)
    {
        try
        {
            _logger.LogInformation("Updating borrower with ID: {Id}", id);
            var existingBorrower = await _borrowerRepository.GetByIdAsync(id);
            if (existingBorrower == null)
            {
                throw new EntityNotFoundException("Borrower", id);
            }
            _mapper.Map(borrowerVM, existingBorrower);
            await _borrowerRepository.UpdateAsync(existingBorrower);
            _logger.LogInformation("Updated borrower with ID: {Id}", id);
            return _mapper.Map<BorrowerVM>(existingBorrower);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error updating borrower with ID {Id}: {Message}", id, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating borrower with ID {Id}", id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting borrower with ID: {Id}", id);
            var borrower = await _borrowerRepository.GetByIdAsync(id);
            if (borrower == null)
            {
                throw new EntityNotFoundException("Borrower", id);
            }

            var activeBorrows = await _borrowRepository.GetAllAsync();
            if (activeBorrows.Any(b => b.BorrowerId == id && b.Status != BorrowStatus.Returned))
            {
                throw new BorrowerHasActiveBorrowsException(id);
            }
            await _borrowerRepository.DeleteAsync(borrower);
            return true;
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business error deleting borrower with ID {Id}: {Message}", id, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting borrower with ID {Id}", id);
            return false;
        }
    }
}