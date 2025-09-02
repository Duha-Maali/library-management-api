using System.ComponentModel.DataAnnotations;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Library Cataloger")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return categories is not null
                ? Ok(categories)
                : BadRequest("No categories found");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([Required][FromRoute][Range(1, int.MaxValue)] int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category is not null
                ? Ok(category)
                : BadRequest($"Category with ID {id} not found");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryVM category)
        {
            var createdCategory = await _categoryService.CreateAsync(category);
            return createdCategory is not null
                ? CreatedAtAction(nameof(GetById), new { id = createdCategory.CategoryId }, createdCategory)
                : BadRequest("Failed to create category. The name may already exist or the data provided is invalid.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Required][FromRoute][Range(1, int.MaxValue)] int id, [FromBody] CreateCategoryVM category)
        {
            var updatedCategory = await _categoryService.UpdateAsync(id, category);
            return updatedCategory is not null
                ? Ok(updatedCategory)
                : BadRequest($"Failed to update category. Make sure the category exists," +
                $" the name is unique, and the data is valid.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Required][FromRoute][Range(1, int.MaxValue)] int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return result
                ? NoContent()
                : BadRequest("Failed to delete category.It may not exist or is currently assigned to one or more books.");
        }
    }
}
