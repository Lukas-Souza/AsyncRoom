using System.Collections.Concurrent;
using Microsoft.OpenApi;

public class RoomEntitie
{
    public string? TimeZone {get; set;}
    public DateTime DataAtualizacao {get; set;}
    public ConcurrentBag<ParticipantUserAsync>? UserParticiantAsyn {get; set;} 
}