using System.Collections.Concurrent;

public interface IRoomRepository
{
    public  Task<bool> IsExistInRoom(ConcurrentBag<ParticipantUserAsync> participantUserAsyncs, string name);
    public Task<RoomDto> AddAsync(string timeZone);
    public Task<ConcurrentBag<ParticipantUserAsync>> GetAllParticipantInRoom(long idRoom_);
    public Task AddParticipantInRoomAsync(long idRoom, ParticipantUserAsync participantUserAsync);
    public void DeletRoomIfEmpty(long idRoom, RoomEntitie roomEntitie);
    public Task<bool> IsExistRoom(long idRoom);
    public RoomEntitie GetRoomById(long idRoom_);
    public ConcurrentDictionary<long, RoomEntitie> GetRooms();

}