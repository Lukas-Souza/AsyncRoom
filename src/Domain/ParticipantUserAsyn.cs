using System.Net.WebSockets;

public class ParticipantUserAsync
{
    public Guid Id {get; private set;}
    public String? NameTag {get; private set;}
    public String? TimeZone {get; private set;}
    public String? Section {get; private set;}
    public WebSocket? WebSocketAsync{ get; private set;}
    public DateTime DataCriacaoUser {get; private set;}
    
    public ParticipantUserAsync( string _NameTag, String _TimeZone, String _Section, WebSocket _WebSocket)
    {
       this.Id = Guid.NewGuid();
       this.NameTag = _NameTag;
       this.TimeZone = _TimeZone;
       this.Section = _Section;
       this.WebSocketAsync = _WebSocket;
       this.DataCriacaoUser = DateTime.Now;

    }
}