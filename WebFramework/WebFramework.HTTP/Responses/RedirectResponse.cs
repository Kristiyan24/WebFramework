namespace WebFramework.HTTP.Responses;

/// <summary>
/// Represents a Redirect Response
/// </summary>
public class RedirectResponse : HttpResponse
{
    public RedirectResponse(string newLocation)
    {
        StatusCode = HttpResponseCode.Found;
        Headers.Add(new Header("Location", newLocation));
    }
}
