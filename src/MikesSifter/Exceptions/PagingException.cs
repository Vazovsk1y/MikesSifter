namespace MikesSifter.Exceptions;

public class PagingException : MikesSifterException
{
    internal PagingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    private PagingException(string? message) : base(message)
    {
    }

    internal static PagingException PageIndexMustBeGreaterThanZero()
    {
        return new PagingException("Page index must be greater than zero.");
    }
    
    internal static PagingException PageSizeMustBeGreaterThanZero()
    {
        return new PagingException("Page size must be greater than zero.");
    }
}