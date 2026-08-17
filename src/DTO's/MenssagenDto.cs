public class MenssagenDto
{
    public Guid Id {get; set;}
    public long idRoom {get;set;}
    public RemetentDto Remetente {get; set;}
    public DateTime DataTimeSendMenssagen {get; set;}
    public String ConteudoMenssage {get;set;}


}