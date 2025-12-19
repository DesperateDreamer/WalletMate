namespace WalletMate.Application.Exceptions;

public sealed class EntityNotFoundException(string entityName, object key)
    : ApplicationExceptionBase($"{entityName} with key '{key}' was not found.")
{
    public string EntityName { get; } = entityName;
    public object Key { get; } = key;
}