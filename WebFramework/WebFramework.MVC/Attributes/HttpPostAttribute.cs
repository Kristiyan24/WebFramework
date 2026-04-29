namespace WebFramework.MVC.Attributes;

public class HttpPostAttribute : HttpMethodAttribute
{
    public override HttpMethodType Type => HttpMethodType.Post;
    
    public HttpPostAttribute()
    {
    }

    public HttpPostAttribute(string url) : base(url)
    {

    }
}
