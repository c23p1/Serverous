using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISessionsRepository
{
	Task<Session?> GetByIdAsync(Guid id);
	Task<bool> HasActiveSessionForServerAsync(Guid serverId);
	void Add(Session session);
	void Update(Session session);
}
