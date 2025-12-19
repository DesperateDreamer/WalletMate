namespace WalletMate.Application.Exceptions;

public sealed class ForbiddenOperationException(string message) : ApplicationExceptionBase(message);