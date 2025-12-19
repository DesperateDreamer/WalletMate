using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletMate.Application.Models.Category;
using WalletMate.Application.Ports.In;

namespace WalletMate.Adapters.In.API.Controllers;

/// <summary>
/// Controller for managing category-related operations.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of all categories.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The category with the specified ID.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await categoryService.GetCategoryByIdAsync(id, cancellationToken);
        return Ok(category);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="dto">The data transfer object containing category details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the newly created category.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAndUpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var newCategoryId = await categoryService.CreateCategoryAsync(dto, cancellationToken);
        return Ok(newCategoryId);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The unique identifier of the category to update.</param>
    /// <param name="dto">The data transfer object containing updated category details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the updated category.</returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateAndUpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var updatedCategoryId = await categoryService.UpdateCategoryAsync(id, dto, cancellationToken);
        return Ok(updatedCategoryId);
    }

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await categoryService.DeleteCategoryAsync(id, cancellationToken);
        return Ok();
    }
}