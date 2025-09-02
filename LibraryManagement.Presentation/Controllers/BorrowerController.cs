using System.ComponentModel.DataAnnotations;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryManagement.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Librarian")]
public class BorrowerController : ControllerBase
{
    private readonly IBorrowerService _borrowerService;

    public BorrowerController(IBorrowerService borrowerService)
    {
        _borrowerService = borrowerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var borrowers = await _borrowerService.GetAllAsync();
        return borrowers is not null
            ? Ok(borrowers)
            : BadRequest("No borrowers found.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var borrower = await _borrowerService.GetByIdAsync(id);
        return borrower is not null
            ? Ok(borrower)
            : BadRequest($"Borrower with ID {id} not found.");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBorrowerVM borrower)
    {
        var createdBorrower = await _borrowerService.CreateAsync(borrower);
        return createdBorrower is not null
            ? CreatedAtAction(nameof(GetById), new { id = createdBorrower.BorrowerId }, createdBorrower)
            : BadRequest("Failed to create borrower. Please ensure all required fields are valid.");      
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([Required][FromRoute][Range(1, int.MaxValue)] int id, [FromBody] CreateBorrowerVM borrower)
    {
        var updatedBorrower = await _borrowerService.UpdateAsync(id, borrower);
        return updatedBorrower is not null
            ? Ok(updatedBorrower)
            : BadRequest("Failed to update borrower.Make sure the borrower exists and the data provided is valid.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var result = await _borrowerService.DeleteAsync(id);
        return result
            ? Ok($"Borrower with ID {id} deleted successfully.")
            : BadRequest("Failed to delete borrower. The borrower may not exist or has active borrow records that must be returned first.");
    }
}
