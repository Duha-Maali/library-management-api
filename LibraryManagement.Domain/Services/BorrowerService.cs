using AutoMapper;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.IRepositories;
using LibraryManagement.Data.Models;

namespace LibraryManagement.Business.Services;

public class BorrowerService : IBorrowerService
{
    private readonly IGenericRepository<Borrower> _borrowerRepository;
    private readonly IGenericRepository<Borrow> _borrowRepository;
    private readonly IMapper _mapper;

    public BorrowerService(
        IGenericRepository<Borrower> borrowerRepository,
        IGenericRepository<Borrow> borrowRepository,
        IMapper mapper)
    {
        _borrowerRepository = borrowerRepository;
        _borrowRepository = borrowRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BorrowerVM>> GetAllAsync()
    {
        var borrowers = await _borrowerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BorrowerVM>>(borrowers);
    }

    public async Task<BorrowerVM?> GetByIdAsync(int id)
    {
        var borrower = await _borrowerRepository.GetByIdAsync(id);
        if (borrower == null) 
            return null;
        return _mapper.Map<BorrowerVM>(borrower);
    }

    public async Task<BorrowerVM> CreateAsync(BorrowerVM borrowerVM)
    {
        var borrower = _mapper.Map<Borrower>(borrowerVM);
        await _borrowerRepository.AddAsync(borrower);
        return _mapper.Map<BorrowerVM>(borrower);
    }

    public async Task<BorrowerVM> UpdateAsync(int id, BorrowerVM borrowerVM)
    {
        var existingBorrower = await _borrowerRepository.GetByIdAsync(id);
        if (existingBorrower == null) 
            throw new KeyNotFoundException("Borrower not found.");

        _mapper.Map(borrowerVM, existingBorrower);
        await _borrowerRepository.UpdateAsync(existingBorrower);
        return _mapper.Map<BorrowerVM>(existingBorrower);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var borrower = await _borrowerRepository.GetByIdAsync(id);
        if (borrower == null) 
            return false;

        // Check for active borrows
        var activeBorrows = await _borrowRepository.GetAllAsync();
        if (activeBorrows.Any(b => b.BorrowerId == id && b.Status != BorrowStatus.Returned))
            throw new InvalidOperationException("Cannot delete borrower with active borrows.");

        await _borrowerRepository.DeleteAsync(borrower);
        return true;
    }
}