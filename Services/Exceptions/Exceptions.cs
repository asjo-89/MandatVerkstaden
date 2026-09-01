namespace Services.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}

public class ConflictException(string message) : Exception(message)
{
}

public class BusinessRulesException : Exception
{
    public BusinessRulesException(string message) : base(message)
    {
    }
    public BusinessRulesException(string message, Exception exception) : base(message, exception)
    {
    }
}
