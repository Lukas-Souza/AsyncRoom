using Microsoft.AspNetCore.Mvc;

public interface IRoomController
{
    public Task GetAllMenssagenAsync(String nametag, long salaId, CancellationToken cancellationToken, [FromHeader(Name = "X-Time-Zone")] string? timeZone);
    public Task<RoomDto> CreadtedRoom([FromHeader(Name = "X-Time-Zone")] string _timeZone);
}