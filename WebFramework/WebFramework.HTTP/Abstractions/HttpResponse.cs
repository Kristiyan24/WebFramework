namespace WebFramework.HTTP.Abstractions;

/// <summary>
/// Represents an HTTP Response
/// </summary>
public class HttpResponse
{
    /// <summary>
    /// HTTP Response status Line Version
    /// </summary>
    public HttpVersionType Version { get; set; }

    /// <summary>
    /// HTTP Response status Line Status Code
    /// </summary>
    public HttpResponseCode StatusCode { get; set; }

    /// <summary>
    /// Collection of HTTP Response Headers
    /// </summary>
    public IList<Header> Headers { get; set; }

    /// <summary>
    /// Collection of HTTP Response Cookies
    /// </summary>
    public IList<ResponseCookie> Cookies { get; set; }

    /// <summary>
    /// HTTP Response Body
    /// </summary>
    public byte[] Body { get; set; }

    /// <summary>
    /// Returns formatted HTTP Response for the browser.
    /// </summary>

    public HttpResponse(HttpResponseCode statusCode, byte[] body) : this()
    {
        Body = body;
        StatusCode = statusCode;
        if (body?.Length > 0) {
            Headers.Add(new Header("Content-Length", body.Length.ToString()));
        }
    }

    internal HttpResponse()
    {
        Version = HttpVersionType.Http10;
        Headers = new List<Header>();
        Cookies = new List<ResponseCookie>();
    }
    
    public override string ToString()
    {
        var responseAsString = new StringBuilder();
        var httpVersionAsString = Version switch
        {
            HttpVersionType.Http10 => "HTTP/1.0",
            HttpVersionType.Http11 => "HTTP/1.1",
            HttpVersionType.Http20 => "HTTP/2.0",
            _ => "HTTP/1.1",
        };

        responseAsString.Append($"{httpVersionAsString} {(int)StatusCode} {StatusCode}" + HttpConstants.NewLine);
        foreach (var header in Headers)
        {
            responseAsString.Append(header.ToString() + HttpConstants.NewLine);
        }

        foreach (var cookie in Cookies)
        {
            responseAsString.Append("Set-Cookie: " + cookie.ToString() + HttpConstants.NewLine); ;
        }

        responseAsString.Append(HttpConstants.NewLine);

        return responseAsString.ToString();
    }
}
