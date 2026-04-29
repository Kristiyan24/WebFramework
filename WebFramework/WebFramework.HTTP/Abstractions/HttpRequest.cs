namespace WebFramework.HTTP.Abstractions;

/// <summary>
/// Represents an HTTP Request
/// </summary>
public class HttpRequest
{
    /// <summary>
    /// HTTP Request Line Method
    /// </summary>
    public HttpMethodType Method { get; set; }

    /// <summary>
    /// HTTP Request Line Version
    /// </summary>
    public HttpVersionType Version { get; set; }

    /// <summary>
    /// Collection of HTTP Request Headers
    /// </summary>
    public IList<Header> Headers { get; set; }

    /// <summary>
    /// Collection of HTTP Request Cookies
    /// </summary>
    public IList<Cookie> Cookies { get; set; }

    /// <summary>
    /// HTTP Request Line Path
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// HTTP Request Body
    /// </summary>
    public string Body { get; set; }
    public string Query { get; set; }
    public IDictionary<string, string> FormData { get; set; }
    public IDictionary<string, string> QueryData { get; set; }
    public IDictionary<string, string> SessionData { get; set; }

    public HttpRequest(string httpRequestAsString)
    {
        if (string.IsNullOrWhiteSpace(httpRequestAsString))
            return;

        Headers = new List<Header>();
        Cookies = new List<Cookie>();

        var lines = httpRequestAsString.Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None);

        var httpInfoHeader = lines[0];
        var infoHeaderParts = httpInfoHeader.Split(' ');
        if (infoHeaderParts.Length != 3) {
            throw new HttpServerException("Invalid HTTP header line.");
        }

        var httpMethod = infoHeaderParts[0];
        Method = httpMethod switch
        {
            "GET" => HttpMethodType.Get,
            "POST" => HttpMethodType.Post,
            "PUT" => HttpMethodType.Put,
            "DELETE" => HttpMethodType.Delete,
            _ => HttpMethodType.Unknown,
        };

        Path = infoHeaderParts[1];

        var httpVersion = infoHeaderParts[2];
        Version = httpVersion switch
        {
            "HTTP/1.0" => HttpVersionType.Http10,
            "HTTP/1.1" => HttpVersionType.Http11,
            "HTTP/2.0" => HttpVersionType.Http20,
            _ => HttpVersionType.Http11,
        };

        bool isInHeader = true;
        StringBuilder bodyBuilder = new StringBuilder();
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) {
                isInHeader = false;
                continue;
            }

            if (isInHeader)
            {
                var headerParts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                if (headerParts.Length != 2) {
                    throw new HttpServerException($"Invalid header: {line}");
                }

                var header = new Header(headerParts[0], headerParts[1]);
                Headers.Add(header);

                if (headerParts[0] == "Cookie")
                {
                    var cookiesAsString = headerParts[1];
                    var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cookieAsString in cookies)
                    {
                        var cookieParts = cookieAsString.Split(new char[] { '=' }, 2);
                        if (cookieParts.Length == 2) {
                            Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
                        }
                    }
                }
            }
            else
            {
                bodyBuilder.AppendLine(line);
            }
        }

        // creator=Niki&tweetName=Hello!
        Body = bodyBuilder.ToString().TrimEnd('\r', '\n');
        FormData = new Dictionary<string, string>();
        ParseData(FormData, Body);

        Query = string.Empty;
        if (Path.Contains("?"))
        {
            var parts = Path.Split(new char[] { '?' }, 2);
            Path = parts[0];
            Query = parts[1];
        }

        QueryData = new Dictionary<string, string>();
        ParseData(QueryData, Query);
    }

    private void ParseData(IDictionary<string, string> output, string input)
    {
        var dataParts = input.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var dataPart in dataParts)
        {
            var parameterParts = dataPart.Split(new char[] { '=' }, 2);
            output.Add(
                HttpUtility.UrlDecode(parameterParts[0]),
                HttpUtility.UrlDecode(parameterParts[1]));
        }
    }
}
