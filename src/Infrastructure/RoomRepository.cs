using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class RoomRepository
{
    private static readonly ConcurrentDictionary<long, RoomEntitie> _room = new();
    public Task<RoomEntitie> GetOrAddAsync(long roomId, string timeZone)
    {
        try
        {
            if (_room.Count >= 10) throw new Exception("Limite de criação de sala atingida");

            else
            {
                
        var connectionSyncRoom = _room.GetOrAdd(roomId, _=>  new RoomEntitie
        {
            TimeZone = timeZone,
            DataAtualizacao = DateTime.Now,
            UserParticiantAsyn = new ConcurrentBag<ParticipantUserAsync>()
        });
        return Task.FromResult(connectionSyncRoom);
        }
        }

        catch (Exception err)
        {
            
            throw new Exception("Erro: "+ err);
        };
    }
    public async Task<ConcurrentBag<ParticipantUserAsync>> GetAllParticipantInRoom(int idRoom_)
    {
        try
        {
            if (!_room.TryGetValue(idRoom_, out var room))
            {
                
            }
            var allPartcipant = room.UserParticiantAsyn ?? new ConcurrentBag<ParticipantUserAsync>();
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
    public void DeletRoomIfEmpty(int idRoom)
    {
        try
        {
            if (!_room.TryGetValue(idRoom,out var room ))
            {
                return;                
            }
            if(room.UserParticiantAsyn?.IsEmpty == true)
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
}