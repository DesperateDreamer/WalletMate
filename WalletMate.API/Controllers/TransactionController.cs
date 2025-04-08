using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.DTOs;
using WalletMate.BLL.Shared.Enums;

namespace WalletMate.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactions(CancellationToken cancellationToken)
    {
        var transactions = await transactionService.GetAllTransactionsAsync(cancellationToken);
        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> GetTransactionById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await transactionService.GetTransactionByIdAsync(id, cancellationToken);
        return Ok(transaction);
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByAccount(Guid accountId, [FromQuery] TransactionSortOption sortBy,
        CancellationToken cancellationToken)
    {
        var transactions = await transactionService.GetTransactionsByAccountAsync(accountId, sortBy, cancellationToken);
        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTransaction([FromBody] CreateTransactionDto dto,
        CancellationToken cancellationToken)
    {
        var newId = await transactionService.CreateTransactionAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetTransactionById), new { id = newId }, newId);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateTransaction(Guid id, [FromBody] UpdateTransactionDto dto,
        CancellationToken cancellationToken)
    {
        var updatedId = await transactionService.UpdateTransactionAsync(id, dto, cancellationToken);
        return Ok(updatedId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id, CancellationToken cancellationToken)
    {
        await transactionService.DeleteTransactionAsync(id, cancellationToken);
        return Ok();
    }
}