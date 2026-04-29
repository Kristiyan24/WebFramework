namespace WebFramework.HTTP.Abstractions;

/// <summary>
/// Represents an HTTP Cookie Information
/// </summary>
public class Cookie
{
    /// <summary>
    /// HTTP Cookie Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// HTTP Cookie Value
    /// </summary>
    public string Value { get; set; }

    public Cookie(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
