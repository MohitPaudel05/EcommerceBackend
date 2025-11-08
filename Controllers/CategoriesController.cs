using Ecommerce.Dtos;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var dtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ProductNames = c.Products.Select(p => p.Name).ToList()
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();

        var dto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductNames = category.Products.Select(p => p.Name).ToList()
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
        await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateDto dto)
    {
        var existing = await _categoryService.GetCategoryByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Name = dto.Name;
        existing.Description = dto.Description;

        await _categoryService.UpdateCategoryAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
}
