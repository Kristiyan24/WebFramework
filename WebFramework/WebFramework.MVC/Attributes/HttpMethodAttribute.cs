namespace WebFramework.MVC.Attributes;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class HttpMethodAttribute : Attribute
{
    public string Url { get; }
    public abstract HttpMethodType Type { get; }

    protected HttpMethodAttribute()
    {

    }

    protected HttpMethodAttribute(string url)
    {
        Url = url;
    }
}
