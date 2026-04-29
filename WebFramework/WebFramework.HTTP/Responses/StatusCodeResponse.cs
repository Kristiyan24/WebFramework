namespace WebFramework.HTTP.Responses;

/// <summary>
/// Represents an Status Code Response
/// </summary>
public class StatusCodeResponse : HttpResponse
{
    public StatusCodeResponse(HttpResponseCode code)
    {
        StatusCode = code;
    }
}
