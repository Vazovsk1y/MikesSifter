namespace MikesSifter.Exceptions;

public class MikesSifterException : Exception
{
    internal MikesSifterException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
    
    internal MikesSifterException(string? message)
        : base(message)
    {
    }
}