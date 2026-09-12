using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StackExchange.Redis;
public class RoomRepository: IRoomRepository
{
    private static readonly ConcurrentDictionary<long, RoomEntitie> _room = new();
    
    private readonly IDatabase _Db;

    public RoomRepository(IConnectionMultiplexer connection)
    {
        this._Db = connection.GetDatabase();
    }

    public async Task<RoomDto> AddAsync( string timeZone)
    {
        try
        {
            if (_room.Count >= 10) throw new Exception("Limite de criação de sala atingida");

        else
        {

        long keyIdRomAync = Random.Shared.Next(00000, 99999);    
        var ObjetRoom = new RoomEntitie
        {
            TimeZone = timeZone,
            DataAtualizacao = DateTime.Now,
            UserParticiantAsyn = new ConcurrentBag<ParticipantUserAsync>()

        };
        string JsonObject =  JsonSerializer.Serialize(ObjetRoom);
        try
        {
        
        await _Db.StringSetAsync( keyIdRomAync.ToString(),JsonObject);
        _room.TryAdd(keyIdRomAync, ObjetRoom);
        return new RoomDto
        {
            Id = keyIdRomAync,
            DataAtualizacao = _room[keyIdRomAync].DataAtualizacao,
            TimeZone = _room[keyIdRomAync].TimeZone,
            UserParticiantAsyn = _room[keyIdRomAync].UserParticiantAsyn,
            StatusSave = "Sala salva com sucesso",    
            UrlRedirect = ""         
        };
        }
        catch (Exception err)
        {
          return new RoomDto{
            
            StatusSave = "Falha ao realizar o salvamento",
            MenssagenErro = err.Message
        };  
            
        }
        
        }
        }

        catch (Exception err)
        {
            
            throw new Exception("Erro: "+ err);
        };
    }
    public async Task<ConcurrentBag<ParticipantUserAsync>> GetAllParticipantInRoom(long idRoom_)
    {
        try
        {
            if (!_room.TryGetValue(idRoom_, out var room))
            {
                
            }
            var resulQuery = _Db.StringGet(idRoom_.ToString()).ToString();     
            if( String.IsNullOrEmpty(resulQuery))  throw new Exception("Não foi possivel adicionar o usuário");  
            var roomResultDeserializer = JsonSerializer.Deserialize<RoomRedisDto>(resulQuery!);
            var Seriallizer =  new ConcurrentBag<ParticipantUserAsync>(roomResultDeserializer.UserParticiantAsyn);
            return await Task.FromResult(Seriallizer);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    public async Task<Task> AddParticipantInRoomAsync(long idRoom, ParticipantUserAsync participantUserAsync)
    {
        Console.WriteLine("Entrou");
        try
        {
            var resulQuery = _Db.StringGet(idRoom.ToString()).ToString();     
            if( String.IsNullOrEmpty(resulQuery))  throw new Exception("Não foi possivel adicionar o usuário");  
            var roomResultDeserializer = JsonSerializer.Deserialize<RoomRedisDto>(resulQuery!);
            

            roomResultDeserializer?.UserParticiantAsyn?.Add(participantUserAsync);
            Console.WriteLine(participantUserAsync.Id);

            var a = new RoomEntitie
            {
                TimeZone = roomResultDeserializer.TimeZone,
                DataAtualizacao = roomResultDeserializer.DataAtualizacao,
                UserParticiantAsyn = [.. roomResultDeserializer.UserParticiantAsyn]
            };

            var serializerObjectSetDb = JsonSerializer.Serialize(a);
            _Db.StringSet(idRoom.ToString(),  serializerObjectSetDb);
            Console.WriteLine("User salvo com sucesso");
            foreach (var item in roomResultDeserializer.UserParticiantAsyn)
            {
                Console.WriteLine("TAG: "+item.NameTag);
                Console.WriteLine("ID :"+item.Id);

            }
            return Task.CompletedTask;
                 
            
           

        }
        catch (Exception err)
        {
            Console.WriteLine("Deu erro nessa porra aqui: "+ err.Message);
            
            throw ;
        }
    }
    public void DeletRoomIfEmpty(long idRoom, RoomEntitie roomEntitie )
    {
        try
        {
            TimeSpan diferenca = roomEntitie.DataAtualizacao - DateTime.Now;

            if (!_room.TryGetValue(idRoom,out var room ))
            {
                return;                
            }
            else if((room.UserParticiantAsyn?.Count == 0) && (diferenca.TotalMinutes > 5))
            {
                _room.TryRemove(idRoom, out _);
                return;
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    public async Task<bool> IsExistInRoom(ConcurrentBag<ParticipantUserAsync> participantUserAsyncs, string name)

{
        return participantUserAsyncs.Any(x => string.Equals(x.NameTag,name, StringComparison.OrdinalIgnoreCase));  

}
         

    public async Task<bool> IsExistRoom(long idRoom){
       return _room.ContainsKey(idRoom);     

    }   
    public RoomEntitie GetRoomById(long idRoom_){
        return _room[idRoom_];
    } 
    public ConcurrentDictionary<long, RoomEntitie> GetRooms(){
        return _room;
    }
}