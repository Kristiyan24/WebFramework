namespace WebFramework.HTTP.Responses;

/// <summary>
/// Represents a File Response
/// </summary>
public class FileResponse : HttpResponse
{
    public FileResponse(byte[] fileContent, string contentType)
    {
        Body = fileContent;
        StatusCode = HttpResponseCode.Ok;
        Headers.Add(new Header("Content-Type", contentType));
        Headers.Add(new Header("Content-Length", Body.Length.ToString()));
    }
}
