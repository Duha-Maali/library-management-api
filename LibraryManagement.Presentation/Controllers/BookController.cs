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
        return books is not null
            ? Ok(books)
            : BadRequest("No books found");
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        return book is not null
            ? Ok(book)
            : BadRequest($"Book with ID {id} was not found.");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookVM book)
    {
        
        var createdBook = await _bookService.CreateAsync(book);
        return createdBook is not null
            ? CreatedAtAction(nameof(GetById), new { id = createdBook.BookId }, createdBook)
            : BadRequest("Failed to create book. Please ensure the category exists and data is valid.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([Required][FromRoute][Range(1, int.MaxValue)] int id, [FromBody] CreateBookVM book)
    {
        var updatedBook = await _bookService.UpdateAsync(id, book);
        return updatedBook is not null
            ? Ok(updatedBook)
            : BadRequest("\"Failed to update the book. Please ensure the book exists, " +
            "the category is valid, and the total copies are not less than borrowed copies.\"");
   }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([Required][FromRoute][Range(1, int.MaxValue)] int id)
    {
        var result = await _bookService.DeleteAsync(id);
        return result
            ? NoContent()
            : BadRequest("Failed to delete the book. It may not exist or cannot be deleted due to business constraints.");
    }
}
