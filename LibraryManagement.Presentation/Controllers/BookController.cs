using System.ComponentModel.DataAnnotations;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Library Cataloger")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        if(books == null)
        {
            return NotFound("No books found.");
        }
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) 
            return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookVM book)
    {
        
        var createdBook = await _bookService.CreateAsync(book);
        return createdBook is not null
            ? CreatedAtAction(nameof(GetById), new { id = createdBook.BookId }, createdBook)
            : BadRequest("Invalid data");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([Required][FromRoute][Range(1, int.MaxValue)] int id, [FromBody] BookVM book)
    {
        try
        {
            var updatedBook = await _bookService.UpdateAsync(id, book);
            return Ok(updatedBook);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var result = await _bookService.DeleteAsync(id);
        if (!result) 
            return NotFound();
        return NoContent();
    }
}
