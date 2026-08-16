using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http; 
using System;
public class RoomAppService: IRoomAppService
{
    
    private static readonly ConcurrentDictionary<long, RoomEntitie> _room = new();
    private readonly ILogger<RoomAppService> _logger ;
    public RoomAppService(ILogger<RoomAppService> logger)
    {
        _logger = logger;
    }
    public async Task JoinRoomAsync (long idRoom, string nameTag,WebSocket webSocket, CancellationToken cancellationToken, string timeZone)
    {


        if (String.IsNullOrWhiteSpace(timeZone)) return; 
        else if( String.IsNullOrWhiteSpace(nameTag) ) return;
        else if( webSocket == null ) return;
        else if (idRoom < 0 ) return;
        else
        {

        
        var room = _room[idRoom];
        var Participant = new ParticipantUserAsync(nameTag, timeZone, idRoom.ToString(), webSocket);
        room.UserParticiantAsyn.Add(Participant); 

        var buffer = new Byte[1024*4];
        try
        {
            var result = await webSocket.ReceiveAsync(buffer, cancellationToken);
            while (!result.CloseStatus.HasValue)
            {
                var textMenssagen = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await BroadCastAsync(idRoom, Participant.Id, textMenssagen, cancellationToken, room);
                result = await webSocket.ReceiveAsync(buffer, cancellationToken);
            }
        }            
        
       

        catch (System.Exception)
        {
            
            throw;
        }
    }}
    private async Task BroadCastAsync(long _salaId, Guid remente, string textMenssagen, CancellationToken cancellationToken, RoomEntitie _roomAsync)
    {
     var bytes = Encoding.UTF8.GetBytes(textMenssagen);
        foreach (var socket in _roomAsync.UserParticiantAsyn)
        {

            await socket.WebSocketAsync.SendAsync(bytes, WebSocketMessageType.Text, true,cancellationToken);
        }

    }
    private void RemoveInRoom(long _salaId, long userId)
    {
        if (_room.TryGetValue(_salaId, out var usuarios))
        {
            usuarios.UserParticiantAsyn = new ConcurrentBag<ParticipantUserAsync>(
                usuarios.UserParticiantAsyn.Where(participant => participant != null));

            if (usuarios.UserParticiantAsyn.IsEmpty)
            {
                _room.TryRemove(_salaId, out _);
                _logger.LogInformation("Sala esta removida");
            }

            _logger.LogInformation("Usuário {UsuarioId} saiu da sala {SalaId}", userId, _salaId);
        }
    }
}