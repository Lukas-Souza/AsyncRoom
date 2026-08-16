using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[Controller]")]
public class RoomController: IRoomController
{
    private readonly IRoomAppService _RoomAppService;
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly AcceptWebSocket _AcceptWebSocket;
    public RoomController( IRoomAppService roomAppService,AcceptWebSocket acceptWebSocket, IHttpContextAccessor httpContextAccessor)
    {
        _RoomAppService = roomAppService;
        _HttpContextAccessor = httpContextAccessor;
        _AcceptWebSocket = acceptWebSocket;
    }

    [HttpGet("/{salaId}/{nameTag}")]
    public async Task GetAllMenssagenAsync(String nametag, long salaId, CancellationToken cancellationToken, [FromHeader(Name = "X-Time-Zone")] string? timeZone  )
    {
        
        try
        {
            var httpContext = _HttpContextAccessor.HttpContext;
            var socket = await _AcceptWebSocket.AcceptWebSocketAsync(httpContext);

            await _RoomAppService.JoinRoomAsync((int)salaId,nametag, socket, cancellationToken, timeZone);
        }
        catch (Exception err)
        {
            
            throw new Exception("Ocorreu um erro ao entra na sala: "+ err.Message);
        
        }


    }
}