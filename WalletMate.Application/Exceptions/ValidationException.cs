namespace WalletMate.Application.Exceptions;

public sealed class ValidationException(IDictionary<string, string[]> errors)
    : ApplicationExceptionBase("One or more validation errors occurred.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(errors);

    public ValidationException(string field, string error)
        : this(new Dictionary<string, string[]>
        {
            [field] = [error]
        })
    {
    }
}