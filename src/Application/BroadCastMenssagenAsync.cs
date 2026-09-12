using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

public class BroadCastMenssagenAsync : IBroadCastMenssagenAsync
{
    public async Task SendAllMenssagenAsync(ConcurrentBag<ParticipantUserAsync> allUsers, Guid userRement, string textMenssagen, CancellationToken cancellationToken)
    {


        //string json = JsonSerializer.Serialize(mensagem);
        byte[] bytesEncoding;

        foreach (var socket in allUsers)
        {
            var ObjectResult = new MenssagenDto
            {
                Id = Guid.NewGuid(),
                idRoom = 1,
                Remetente = new RemetentDto
                {
                    Id = socket.Id,
                    NameTag = socket.NameTag,
                    DataUtimaAtualizacao = socket.DataCriacaoUser
                },
                DataTimeSendMenssagen = DateTime.Now,
                ConteudoMenssage = textMenssagen
            };

            bytesEncoding = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ObjectResult));
            if (!(socket.Id == userRement)) await socket.WebSocketAsync.SendAsync(bytesEncoding, WebSocketMessageType.Text, true, cancellationToken);
        }

    }
}