namespace WebFramework.MVC.Attributes;

public class HttpGetAttribute : HttpMethodAttribute
{
    public override HttpMethodType Type => HttpMethodType.Get;

    public HttpGetAttribute()
    {

    }

    public HttpGetAttribute(string url) : base(url)
    {

    }
}
