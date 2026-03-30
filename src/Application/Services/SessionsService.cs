using Application.Common;
using Application.Contracts.Sessions;
using Application.Interfaces;
using Application.Interfaces.Identity;
using Application.Interfaces.Jobs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class SessionsService : ISessionsService
{
	private static readonly TimeSpan ServerStartupDelay = TimeSpan.FromMinutes(5);
	private static readonly TimeSpan SessionDuration = TimeSpan.FromMinutes(20);

	private readonly ISessionsRepository _sessionsRepository;
	private readonly IServersRepository _serversRepository;
	private readonly ISessionJobScheduler _sessionJobScheduler;
	private readonly ISessionStateService _sessionStateService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUser _currentUser;

	public SessionsService
	(
		ISessionsRepository sessionsRepository,
		IServersRepository serversRepository,
		ISessionJobScheduler sessionJobScheduler,
		ISessionStateService sessionStateService,
		IUnitOfWork unitOfWork,
		ICurrentUser currentUser
	)
	{
		_sessionsRepository = sessionsRepository;
		_serversRepository = serversRepository;
		_sessionJobScheduler = sessionJobScheduler;
		_sessionStateService = sessionStateService;
		_unitOfWork = unitOfWork;
		_currentUser = currentUser;
	}

	public async Task<GenericResult<StartSessionResponse>> Start(Guid serverId)
	{
		var server = await _serversRepository.GetByIdAsync(serverId);
		if (server is null)
		{
			return GenericResult<StartSessionResponse>.Failure(Error.EntityNotFound(nameof(Server), serverId.ToString()));
		}
		if (await _sessionsRepository.HasActiveSessionForServerAsync(serverId))
		{
			return GenericResult<StartSessionResponse>.Failure(Error.InvalidOperation("У сервера уже есть активная сессия"));
		}

		var creationTime = DateTime.UtcNow;
		var session = new Session
		{
			UserId = _currentUser.UserId,
			ServerId = serverId,
			IsActive = true,
			CreatedAt = creationTime
		};

		if (server.Status == ServerStatus.Started)
		{
			session.Status = SessionStatus.InUse;
			session.StartTime = creationTime;
			session.EndTime = creationTime.Add(SessionDuration);
		}
		else
		{
			session.Status = SessionStatus.Starting;
			server.Status = ServerStatus.Starting;
		}
		_sessionsRepository.Add(session);
		await _unitOfWork.SaveChangesAsync();

		if (session.Status == SessionStatus.Starting)
		{
			_sessionJobScheduler.ScheduleActivation(session.Id, ServerStartupDelay);
		}
		else
		{
			_sessionJobScheduler.ScheduleExpiration(session.Id, SessionDuration);
		}

		var startSessionResponse = new StartSessionResponse { Id = session.Id };
		return GenericResult<StartSessionResponse>.Success(startSessionResponse);
	}

	public async Task<GenericResult<SessionStatusResponse>> GetStatusByIdAsync(Guid sessionId)
	{
		var session = await _sessionsRepository.GetByIdAsync(sessionId);
		if (session is null)
		{
			return GenericResult<SessionStatusResponse>.Failure(Error.EntityNotFound(nameof(Session), sessionId.ToString()));
		}
		if (session.UserId != _currentUser.UserId)
		{
			return GenericResult<SessionStatusResponse>.Failure(Error.Unauthorized("Не удалось получить статус сессии: сессия не принадлежит текущему пользователю"));
		}
		var sessionStatusResponse = new SessionStatusResponse { Status = session.Status };
		return GenericResult<SessionStatusResponse>.Success(sessionStatusResponse);
	}

	public async Task<Result> Stop(Guid sessionId) =>
		await _sessionStateService.StopByUser(sessionId, _currentUser.UserId);
}
