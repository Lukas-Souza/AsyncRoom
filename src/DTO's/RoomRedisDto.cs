using System.Collections.Concurrent;

public class RoomRedisDto
{
    public long Id {get; set;}
    public string? TimeZone {get; set;}
    public DateTime DataAtualizacao {get; set;}
    public List<ParticipantUserAsync>? UserParticiantAsyn {get; set;} 
    public string? StatusSave {get; set;}
    public String? MenssagenErro {get; set;}
}