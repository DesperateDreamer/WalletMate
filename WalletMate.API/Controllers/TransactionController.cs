using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.BLL.Shared.Enums;

namespace WalletMate.API.Controllers;

/// <summary>
/// Controller for managing transaction-related operations.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    /// <summary>
    /// Retrieves all transactions.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of all transactions.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactions(CancellationToken cancellationToken)
    {
        var transactions = await transactionService.GetAllTransactionsAsync(cancellationToken);
        return Ok(transactions);
    }

    /// <summary>
    /// Retrieves a transaction by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The transaction with the specified ID.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> GetTransactionById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await transactionService.GetTransactionByIdAsync(id, cancellationToken);
        return Ok(transaction);
    }

    /// <summary>
    /// Retrieves transactions for a specific account, optionally sorted.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="sortBy">The sorting option for the transactions.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of transactions for the specified account.</returns>
    [HttpGet("account/{accountId:guid}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByAccount(Guid accountId, [FromQuery] TransactionSortOption sortBy,
        CancellationToken cancellationToken)
    {
        var transactions = await transactionService.GetTransactionsByAccountAsync(accountId, sortBy, cancellationToken);
        return Ok(transactions);
    }

    /// <summary>
    /// Creates a new transaction.
    /// </summary>
    /// <param name="dto">The data transfer object containing transaction details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the newly created transaction.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTransaction([FromBody] CreateTransactionDto dto,
        CancellationToken cancellationToken)
    {
        var newId = await transactionService.CreateTransactionAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetTransactionById), new { id = newId }, newId);
    }

    /// <summary>
    /// Updates an existing transaction.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to update.</param>
    /// <param name="dto">The data transfer object containing updated transaction details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the updated transaction.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateTransaction(Guid id, [FromBody] UpdateTransactionDto dto,
        CancellationToken cancellationToken)
    {
        var updatedId = await transactionService.UpdateTransactionAsync(id, dto, cancellationToken);
        return Ok(updatedId);
    }

    /// <summary>
    /// Deletes a transaction by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A success message if the transaction was deleted.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id, CancellationToken cancellationToken)
    {
        await transactionService.DeleteTransactionAsync(id, cancellationToken);
        return Ok();
    }
}