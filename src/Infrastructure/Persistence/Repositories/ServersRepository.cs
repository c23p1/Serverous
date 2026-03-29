using Application.Contracts.Servers;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ServersRepository : IServersRepository
{
	private readonly DbSet<Server> _servers;

	public ServersRepository(ApplicationDbContext dbContext)
	{
		_servers = dbContext.Set<Server>();
	}

	public async Task<List<Server>> GetAvailableAsync(ServerFilters filters)
	{
		var servers = _servers.Where(s => !s.Sessions.Any(session => session.IsActive));
		servers = servers.Where(s => s.OperatingSystem == filters.OperatingSystem);
		servers = servers.Where(s => s.RamGiB >= filters.MinRamGiB && s.RamGiB <= filters.MaxRamGiB);
		servers = servers.Where(s => s.StorageGiB >= filters.MinStorageGiB && s.StorageGiB <= filters.MaxStorageGiB);
		servers = servers.Where(s => s.CpuCoreCount >= filters.MinCpuCoreCount && s.CpuCoreCount <= filters.MaxCpuCoreCount);
		return await servers.AsNoTracking().ToListAsync();
	}

	public async Task<Server?> GetByIdAsync(Guid id) =>
		await _servers.FindAsync(id);

	public void Add(Server server)
	{
		_servers.Add(server);
	}

	public void Update(Server server) =>
		_servers.Update(server);
}
