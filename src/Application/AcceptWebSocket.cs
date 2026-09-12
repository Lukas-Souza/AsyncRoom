using System.Net.WebSockets;

public class AcceptWebSocket : IAcceptWebSocket
{
    public async Task<WebSocket> AcceptWebSocketAsync(HttpContext _httpContext)
    {
        if (!_httpContext.WebSockets.IsWebSocketRequest)
        {
            _httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        }
        try
        {
            return await _httpContext.WebSockets.AcceptWebSocketAsync();

        }
        catch (System.Exception)
        {

            throw;
        }
    }
}