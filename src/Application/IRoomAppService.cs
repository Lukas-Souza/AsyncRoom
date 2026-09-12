using System.Collections.Concurrent;
using System.Net.WebSockets;

public interface IRoomAppService
{
    public Task JoinRoomAsync(long idRoom, string nameTag, WebSocket webSocket, CancellationToken cancellationToken, string timeZone);
    public Task<RoomDto> CreatedRoomNotExist(string timeZone);
}