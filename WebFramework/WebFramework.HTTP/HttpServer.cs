namespace WebFramework.HTTP;

public class HttpServer : IHttpServer
{
    private readonly ILogger logger;
    private readonly IList<Route> routeTable;
    private readonly TcpListener tcpListener;
    private readonly IDictionary<string, IDictionary<string, string>> sessions;

    public HttpServer(int port, IList<Route> routeTable, ILogger logger)
    {
        tcpListener = new TcpListener(IPAddress.Loopback, port);
        this.routeTable = routeTable;
        this.logger = logger;
        sessions = new Dictionary<string, IDictionary<string, string>>();
    }

    /// <summary>
    /// Resets the HTTP Server asynchronously
    /// </summary>
    public async Task ResetAsync()
    {
        Stop();
        await StartAsync();
    }

    /// <summary>
    /// Starts the HTTP Server asynchronously
    /// </summary>
    public async Task StartAsync()
    {
        tcpListener.Start();
        while (true)
        {
            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(() => ProcessClientAsync(tcpClient));
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }

    /// <summary>
    /// Stops the HTTP Server.
    /// </summary>
    public void Stop()
    {
        tcpListener.Stop();
    }

    /// <summary>
    /// Processes the TCP Client asynchronously and returns HTTP Response for the browser.
    /// </summary>
    private async Task ProcessClientAsync(TcpClient tcpClient)
    {
        using NetworkStream networkStream = tcpClient.GetStream();
        try
        {
            byte[] requestBytes = new byte[1000000]; // TODO: Use buffer
            int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
            string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

            var request = new HttpRequest(requestAsString);
            string newSessionId = null;
            var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionIdCookieName);
            if (sessionCookie != null && sessions.ContainsKey(sessionCookie.Value))
            {
                request.SessionData = sessions[sessionCookie.Value];
            }
            else
            {
                newSessionId = Guid.NewGuid().ToString();
                var dictionary = new Dictionary<string, string>();
                sessions.Add(newSessionId, dictionary);
                request.SessionData = dictionary;
            }

            logger.Log($"{request.Method} {request.Path}");

            var route = routeTable.FirstOrDefault(
                x => x.HttpMethod == request.Method && string.Compare(x.Path, request.Path, true) == 0);
            HttpResponse response;
            if (route == null)
            {
                response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
            }
            else
            {
                response = route.Action(request);
            }

            response.Headers.Add(new Header("Server", "HTTPServer/1.0"));

            if (newSessionId != null)
            {
                response.Cookies.Add(
                    new ResponseCookie(HttpConstants.SessionIdCookieName, newSessionId)
                        { HttpOnly = true, MaxAge = 30*3600, });
            }

            byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            await networkStream.WriteAsync(response.Body, 0, response.Body.Length);
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponse(
                HttpResponseCode.InternalServerError,
                Encoding.UTF8.GetBytes(ex.ToString()));
            errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));
            byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());
            await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
        }
    }
}
