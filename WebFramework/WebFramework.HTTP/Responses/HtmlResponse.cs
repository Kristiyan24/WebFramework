namespace WebFramework.HTTP.Responses;

/// <summary>
/// Represents an HTML Response
/// </summary>
public class HtmlResponse : HttpResponse
{
    public HtmlResponse(string html)
    {
        StatusCode = HttpResponseCode.Ok;
        Body = Encoding.UTF8.GetBytes(html);
        Headers.Add(new Header("Content-Type", "text/html"));
        Headers.Add(new Header("Content-Length", Body.Length.ToString()));
    }
}
