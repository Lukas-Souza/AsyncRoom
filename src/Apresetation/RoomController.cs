using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[Controller]")]
public class RoomController: IRoomController
{
    private readonly IRoomAppService _RoomAppService;
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly IAcceptWebSocket _AcceptWebSocket;
    
    public RoomController(IRoomAppService roomAppService, IAcceptWebSocket acceptWebSocket, IHttpContextAccessor httpContextAccessor)
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

            await _RoomAppService.JoinRoomAsync(salaId,nametag, socket, cancellationToken, timeZone);
        }
        catch (Exception err)
        {
            
            throw new Exception("Ocorreu um erro ao entra na sala: "+ err.Message);
        
        }


    }
    [HttpPost("/criated")]
    public async Task<RoomDto> CreadtedRoom([FromHeader(Name = "X-Time-Zone")] string _timeZone )
    {
        try
        {
        return await _RoomAppService.CreatedRoomNotExist(_timeZone);
            
        }
        catch (Exception err)
        {
            
            throw new Exception("Erro: "+err);
        }
    }
}