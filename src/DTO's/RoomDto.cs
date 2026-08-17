using System.Collections.Concurrent;

public class RoomDto
{
    public long Id {get; set;}
    public string? TimeZone {get; set;}
    public DateTime DataAtualizacao {get; set;}
    public ConcurrentBag<ParticipantUserAsync>? UserParticiantAsyn {get; set;} 
}