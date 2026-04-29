namespace WebFramework.HTTP.Abstractions;

/// <summary>
/// Represents an HTTP Header Information
/// </summary>
public class Header
{
    /// <summary>
    /// HTTP Header Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// HTTP Header Value
    /// </summary>
    public string Value { get; set; }

    public Header(string name, string value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Returns formatted HTTP Header
    /// </summary>
    public override string ToString()
    {
        return $"{Name}: {Value}";
    }
}