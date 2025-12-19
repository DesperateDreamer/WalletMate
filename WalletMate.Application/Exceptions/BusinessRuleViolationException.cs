namespace WalletMate.Application.Exceptions;

public sealed class BusinessRuleViolationException(string message) : ApplicationExceptionBase(message);