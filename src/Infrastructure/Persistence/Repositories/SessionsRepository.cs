using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SessionsRepository : ISessionsRepository
{
	private readonly DbSet<Session> _sessions;

	public SessionsRepository(ApplicationDbContext dbContext)
	{
		_sessions = dbContext.Set<Session>();
	}

	public async Task<Session?> GetByIdAsync(Guid id) =>
		await _sessions
			.Include(s => s.Server)
			.FirstOrDefaultAsync(s => s.Id == id);

	public async Task<bool> HasActiveSessionForServerAsync(Guid serverId) =>
		await _sessions.AnyAsync(s => s.ServerId == serverId && s.IsActive);

	public void Add(Session session) =>
		_sessions.Add(session);

	public void Update(Session session) =>
		_sessions.Update(session);
}
