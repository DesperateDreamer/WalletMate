namespace WalletMate.BLL.Shared.CustomExceptions;

public class NotAcceptableException : Exception
{
    public NotAcceptableException(string message) : base(message)
    {
    }

    public NotAcceptableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}