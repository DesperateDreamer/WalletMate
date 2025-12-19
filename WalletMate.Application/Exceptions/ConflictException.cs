namespace WalletMate.Application.Exceptions;

public sealed class ConflictException(string message) : ApplicationExceptionBase(message);