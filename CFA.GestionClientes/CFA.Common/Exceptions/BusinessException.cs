// Ruta: CFA.Common/Exceptions/BusinessException.cs
namespace CFA.Common.Exceptions;

public class BusinessException : Exception
{
    public BusinessException() : base() { }
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, Exception innerException) 
        : base(message, innerException) { }
}

// En el mismo archivo o en otro archivo NotFoundException.cs pero con el mismo namespace
public class NotFoundException : Exception
{
    public NotFoundException() : base() { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
}
public class ValidationException : Exception
{
    public ValidationException() : base() { }
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception innerException) 
        : base(message, innerException) { }
}