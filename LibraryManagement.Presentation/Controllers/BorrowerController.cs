using System.ComponentModel.DataAnnotations;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        return Ok(borrowers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var borrower = await _borrowerService.GetByIdAsync(id);
        if (borrower == null) 
            return NotFound();
        return Ok(borrower);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BorrowerVM borrower)
    {
        try
        {
            var createdBorrower = await _borrowerService.CreateAsync(borrower);
            return CreatedAtAction(nameof(GetById), new { id = createdBorrower.BorrowerId }, createdBorrower);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([Required][FromRoute][Range(1, int.MaxValue)] int id, [FromBody] BorrowerVM borrower)
    {
        try
        {
            var updatedBorrower = await _borrowerService.UpdateAsync(id, borrower);
            return Ok(updatedBorrower);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        try
        {
            var result = await _borrowerService.DeleteAsync(id);
            if (!result) 
                return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
