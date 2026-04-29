namespace WebFramework.HTTP.Abstractions;

/// <summary>
/// Represents an HTTP Response Cookie with information for Cookie Scope, Cookie Lifetime, Cookie Security and SameSite.
/// </summary>
public class ResponseCookie : Cookie
{
    public ResponseCookie(string name, string value)
        : base(name, value)
    {
        Path = "/";
        SameSite = SameSiteType.None;
        Expires = DateTime.UtcNow.AddDays(30);
    }

    /// <summary>
    /// Cookie scope attribute
    /// Specifies allowed hosts to receive the cookie.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Cookie scope attribute
    /// Indicates a URL path that must exist in the requested URL in order to send the Cookie header.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Cookie lifetime attribute
    /// Sets an expiry date for when a cookie gets deleted.
    /// </summary>
    public DateTime? Expires { get; set; }

    /// <summary>
    /// Cookie lifetime attribute
    /// Sets the time in seconds for when a cookie will be deleted.
    /// </summary>
    public long? MaxAge { get; set; }

    /// <summary>
    /// Cookie security attribute
    /// The secure flag is an option that can be set by the application server when sending a new cookie to the user within an HTTP Response.
    /// The purpose of the secure flag is to prevent cookies from being observed by unauthorized parties due to the transmission of a the cookie in clear text.
    /// To accomplish this goal, browsers which support the secure flag will only send cookies with the secure flag when the request is going to a HTTPS page.
    /// Said in another way, the browser will not send a cookie with the secure flag set over an unencrypted HTTP request.
    /// </summary>
    public bool Secure { get; set; }

    /// <summary>
    /// Cookie security attribute
    /// Helps mitigate the risk of client side script accessing the protected cookie.
    /// If the flag is included in the HTTP response header, the cookie cannot be accessed through client side script
    /// </summary>
    public bool HttpOnly { get; set; }

    /// <summary>
    /// Cookie security attribute
    /// SameSite prevents the browser from sending this cookie along with cross-site requests. 
    /// The main goal is mitigate the risk of cross-origin information leakage. It also provides some protection against cross-site request forgery attacks.
    /// </summary>
    public SameSiteType SameSite { get; set; }

    //------------ PUBLIC METHODS ------------
    /// <summary>
    /// Returns formatted HTTP Response Cookie.
    /// </summary>
    public override string ToString()
    {
        StringBuilder cookieBuilder = new StringBuilder();
        cookieBuilder.Append($"{Name}={Value}");
        
        if (MaxAge.HasValue) {
            cookieBuilder.Append($"; Max-Age=" + MaxAge.Value.ToString());
        }
        else if (Expires.HasValue) {
            cookieBuilder.Append($"; Expires=" + Expires.Value.ToString("R"));
        }

        if (!string.IsNullOrWhiteSpace(Domain)) {
            cookieBuilder.Append($"; Domain=" + Domain);
        }

        if (!string.IsNullOrWhiteSpace(Path)) {
            cookieBuilder.Append($"; Path=" + Path);
        }

        if (Secure) {
            cookieBuilder.Append("; Secure");
        }

        if (HttpOnly) {
            cookieBuilder.Append("; HttpOnly");
        }

        cookieBuilder.Append("; SameSite=" + SameSite.ToString());

        return cookieBuilder.ToString();
    }
}
