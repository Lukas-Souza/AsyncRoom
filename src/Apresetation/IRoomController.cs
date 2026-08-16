using Microsoft.AspNetCore.Mvc;

public interface IRoomController
{
    Task GetAllMenssagenAsync(String nametag, long salaId, CancellationToken cancellationToken,[FromHeader(Name = "X-Time-Zone")] string? timeZone   );
    
}