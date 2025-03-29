using Microsoft.AspNetCore.Mvc;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await categoryService.GetCategoryByIdAsync(id, cancellationToken);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAndUpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var newCategoryId = await categoryService.CreateCategoryAsync(dto, cancellationToken);
        return Ok(newCategoryId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateAndUpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var updatedCategoryId = await categoryService.UpdateCategoryAsync(id, dto, cancellationToken);
        return Ok(updatedCategoryId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await categoryService.DeleteCategoryAsync(id, cancellationToken);
        return Ok(deleted);
    }
}