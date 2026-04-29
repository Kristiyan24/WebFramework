namespace WebFramework.HTTP.Exceptions;

/// <summary>
/// Represents errors that occur during HTTP Request and HTTP Response parsing
/// </summary>
public class HttpServerException : Exception
{
    public HttpServerException(string message) : base(message)
    {
        
    }
}
