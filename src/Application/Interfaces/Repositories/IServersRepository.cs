using Application.Contracts.Servers;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IServersRepository
{
	Task<List<Server>> GetAvailableAsync(ServerFilters filters);
	Task<Server?> GetByIdAsync(Guid id);
	void Add(Server server);
	void Update(Server server);
}
