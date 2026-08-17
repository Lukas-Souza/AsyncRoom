using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class RoomRepository: IRoomRepository
{
    private static readonly ConcurrentDictionary<long, RoomEntitie> _room = new();
    public async Task<RoomDto> AddAsync( string timeZone)
    {
        try
        {
            if (_room.Count >= 10) throw new Exception("Limite de criação de sala atingida");

        else
        {

         long keyIdRomAync = Random.Shared.Next(00000, 99999);    

        _room.TryAdd(keyIdRomAync, new RoomEntitie
        {
            TimeZone = timeZone,
            DataAtualizacao = DateTime.Now,
            UserParticiantAsyn = new ConcurrentBag<ParticipantUserAsync>()
        });
        Console.WriteLine(_room.Count);

        return new RoomDto
        {
            Id = keyIdRomAync,
            DataAtualizacao = _room[keyIdRomAync].DataAtualizacao,
            TimeZone = _room[keyIdRomAync].TimeZone,
            UserParticiantAsyn = _room[keyIdRomAync].UserParticiantAsyn             
        };
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
            var allPartcipant = room?.UserParticiantAsyn ?? new ConcurrentBag<ParticipantUserAsync>();
            return await Task.FromResult(allPartcipant);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    public Task AddParticipantInRoomAsync(long idRoom, ParticipantUserAsync participantUserAsync)
    {
        try
        {
            if(!_room.TryGetValue(idRoom, out var room))
            {
            throw new Exception("Não foi possivel adicionar o usuário");     
            }
            room.UserParticiantAsyn?.Add(participantUserAsync);
            return Task.CompletedTask;

        }
        catch (System.Exception)
        {
            
            throw;
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