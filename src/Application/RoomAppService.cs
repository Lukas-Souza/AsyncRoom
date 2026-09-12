using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.Eventing.Reader;
public class RoomAppService : IRoomAppService
{
    private readonly ILogger<RoomAppService> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IBroadCastMenssagenAsync _broadCastMenssagenAsync;
    public RoomAppService(ILogger<RoomAppService> logger, IRoomRepository roomRepository, IBroadCastMenssagenAsync broadCastMenssagenAsync)
    {
        _logger = logger;
        _roomRepository = roomRepository;
        _broadCastMenssagenAsync = broadCastMenssagenAsync;
    }
    public async Task JoinRoomAsync(long idRoom, string nameTag, WebSocket webSocket, CancellationToken cancellationToken, string timeZone)
    {
        if (String.IsNullOrWhiteSpace(timeZone)) throw new Exception("Erro: TimeZone nulla");
        else if (String.IsNullOrWhiteSpace(nameTag)) throw new Exception("Erro: nameTag nulla");
        else if (webSocket == null) throw new Exception("Erro: WebScoket nulla");
        else if (idRoom < 0) throw new Exception("Erro: o Id da sala não pode ser nulla");
        if (!await _roomRepository.IsExistRoom(idRoom)) throw new Exception("Erro: Sala não econtarada");
        else
        {
            var allUsersInRoom = await _roomRepository.GetAllParticipantInRoom(idRoom);
            if (await _roomRepository.IsExistInRoom(allUsersInRoom, nameTag)) throw new Exception("Erro: Key duplicada");
            var rooms = _roomRepository.GetRooms();
            var roomById = rooms[idRoom];
            if (!await _roomRepository.IsExistRoom(idRoom)) throw new Exception("Sala não encontrada.");
            var Participant = new ParticipantUserAsync(nameTag, timeZone, idRoom.ToString(), webSocket);
            _roomRepository.AddParticipantInRoomAsync(idRoom, Participant);
            if (roomById?.UserParticiantAsyn != null)
            {
                roomById.UserParticiantAsyn.Add(Participant);
                roomById.DataAtualizacao = DateTime.Now;
            }
            var buffer = new Byte[1024 * 4];
            using var heartbeatCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var heartbeatTask = StartHeartbeatAsync(webSocket, heartbeatCts.Token);
            try
            {
                var result = await webSocket.ReceiveAsync(buffer, cancellationToken);
                while (!result.CloseStatus.HasValue)
                {
                    _roomRepository.DeletRoomIfEmpty(idRoom, roomById);
                    var textMenssagen = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    await _broadCastMenssagenAsync.SendAllMenssagenAsync(allUsersInRoom, Participant.Id, textMenssagen, cancellationToken);
                    result = await webSocket.ReceiveAsync(buffer, cancellationToken);
                }
            }

            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                heartbeatCts.Cancel();
            }
        }
    }

    private async Task StartHeartbeatAsync(WebSocket webSocket, CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested && webSocket.State == WebSocketState.Open)
            {
                await Task.Delay(TimeSpan.FromSeconds(20), token);
                if (webSocket.State == WebSocketState.Open)
                {
                    var pingBytes = Encoding.UTF8.GetBytes("{\"type\":\"ping\"}");
                    await webSocket.SendAsync(pingBytes, WebSocketMessageType.Text, true, token);
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (WebSocketException)
        {
        }
    }
    public async Task<RoomDto> CreatedRoomNotExist(string timeZone)
    {
        return await _roomRepository.AddAsync(timeZone);
    }
}