namespace WalletMate.Adapters.In.API.Middleware;

public class ErrorResponse
{
    public List<string> Errors { get; init; } = [];

    public static ErrorResponse From(string message)
    {
        return new ErrorResponse
        {
            Errors = string.IsNullOrWhiteSpace(message) ? ["Unknown error."] : [message]
        };
    }

    public static ErrorResponse From(IEnumerable<string> errors)
    {
        var list = errors?
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToList() ?? [];

        return new ErrorResponse
        {
            Errors = list.Count == 0 ? ["Unknown error."] : list
        };
    }

    public static ErrorResponse From(IReadOnlyDictionary<string, string[]>? errors)
    {
        var list = new List<string>();

        if (errors is null)
        {
            return new ErrorResponse
            {
                Errors = list.Count == 0 ? ["Validation failed."] : list
            };
        }

        foreach (var (field, messages) in errors)
        {
            if (messages.Length == 0)
            {
                if (!string.IsNullOrWhiteSpace(field))
                    list.Add(field);

                continue;
            }

            list.AddRange(from msg in messages
                where !string.IsNullOrWhiteSpace(msg)
                select string.IsNullOrWhiteSpace(field) ? msg : $"{field}: {msg}");
        }

        return new ErrorResponse
        {
            Errors = list.Count == 0 ? ["Validation failed."] : list
        };
    }
}