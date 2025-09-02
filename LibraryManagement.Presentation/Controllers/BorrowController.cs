using System.ComponentModel.DataAnnotations;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.Services;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Librarian")]
public class BorrowController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var borrows = await _borrowService.GetAllAsync();
        return borrows is not null
            ? Ok(borrows)
            : BadRequest("No borrows found.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var borrow = await _borrowService.GetByIdAsync(id);
        return borrow is not null
            ? Ok(borrow)
            : BadRequest($"Borrow with ID {id} was not found.");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBorrowVM borrow)
    {
        var createdBorrow = await _borrowService.CreateAsync(borrow);
        return createdBorrow is not null
            ? CreatedAtAction(nameof(GetById), new { id = createdBorrow.BorrowId }, createdBorrow)
            : BadRequest("Failed to create borrow. Make sure the borrower and book exist, " +
            "there are valid Total copies, and the due date is after the borrow date.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([Required][FromRoute][Range(1, int.MaxValue)] int id, [FromBody] UpdateBorrowVM borrow)
    {
        var updatedBorrow = await _borrowService.UpdateAsync(id, borrow);
        return updatedBorrow is not null
            ? Ok(updatedBorrow)
            : BadRequest("Failed to update borrow. Ensure the borrow record exists and the return date is valid.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var result = await _borrowService.DeleteAsync(id);
        return result
            ? NoContent()
            : BadRequest("Failed to delete borrow. The record may not exist or the book has not been returned yet.");
    }
}
