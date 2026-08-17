using System.Collections.Concurrent;

public interface IBroadCastMenssagenAsync
{
    public Task SendAllMenssagenAsync(ConcurrentBag<ParticipantUserAsync> allUsers, Guid userRement, string textMenssagen, CancellationToken cancellationToken);
}