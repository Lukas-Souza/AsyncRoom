using System.Net.WebSockets;

public interface IAcceptWebSocket
{
    public Task<WebSocket> AcceptWebSocketAsync(HttpContext _httpContext);
}