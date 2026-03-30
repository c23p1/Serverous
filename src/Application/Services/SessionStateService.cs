using Application.Common;
using Application.Interfaces;
using Application.Interfaces.Jobs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class SessionStateService : ISessionStateService
{
	public static readonly TimeSpan SessionDuration = TimeSpan.FromMinutes(20);

	private readonly ISessionsRepository _sessionsRepository;
	private readonly ISessionJobScheduler _sessionJobScheduler;
	private readonly IUnitOfWork _unitOfWork;

	public SessionStateService(ISessionsRepository sessionsRepository, ISessionJobScheduler sessionJobScheduler, IUnitOfWork unitOfWork)
	{
		_sessionsRepository = sessionsRepository;
		_sessionJobScheduler = sessionJobScheduler;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Activate(Guid sessionId)
	{
		var sessionResult = await GetById(sessionId);
		if (!sessionResult.IsSuccess)
		{
			return Result.Failure(sessionResult.Error!);
		}

		var session = sessionResult.Value;
		if (!session.IsActive || session.Status != SessionStatus.Starting)
		{
			return Result.Success();
		}
		if (session.Server is null)
		{
			return Result.Failure(Error.InvalidOperation("Для сессии не найден сервер"));
		}

		var startTime = DateTime.UtcNow;
		session.Status = SessionStatus.InUse;
		session.StartTime = startTime;
		session.EndTime = startTime.Add(SessionDuration);
		session.Server.Status = ServerStatus.Started;

		await _unitOfWork.SaveChangesAsync();
		_sessionJobScheduler.ScheduleExpiration(session.Id, SessionDuration);
		return Result.Success();
	}

	public async Task<Result> Expire(Guid sessionId)
	{
		var sessionResult = await GetById(sessionId);
		if (!sessionResult.IsSuccess)
		{
			return Result.Failure(sessionResult.Error!);
		}

		var session = sessionResult.Value;
		if (!session.IsActive || session.Status != SessionStatus.InUse)
		{
			return Result.Success();
		}
		session.IsActive = false;
		session.Status = SessionStatus.Expired;
		session.Server!.Status = ServerStatus.Stopped;

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}

	public async Task<Result> StopByUser(Guid sessionId, Guid userId)
	{
		var sessionResult = await GetById(sessionId);
		if (!sessionResult.IsSuccess)
		{
			return Result.Failure(sessionResult.Error!);
		}

		var session = sessionResult.Value;
		if (session.UserId != userId)
		{
			return Result.Failure(Error.Unauthorized("Невозможно освободить сервер: сессия не принадлежит текущему пользователю"));
		}
		if (!session.IsActive)
		{
			return Result.Success();
		}

		var isInStartingState = session.Status == SessionStatus.Starting;
		session.IsActive = false;
		session.Status = SessionStatus.Stopped;

		if (isInStartingState && session.Server is not null)
		{
			session.Server.Status = ServerStatus.Stopped;
		}

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}

	private async Task<GenericResult<Session>> GetById(Guid sessionId)
	{
		var session = await _sessionsRepository.GetByIdAsync(sessionId);
		if (session is null)
		{
			return GenericResult<Session>.Failure(Error.EntityNotFound(nameof(Session), sessionId.ToString()));
		}
		return GenericResult<Session>.Success(session);
	}
}
