using Application.Common;

namespace Application.Interfaces.Services;

public interface ISessionStateService
{
	Task<Result> Activate(Guid sessionId);
	Task<Result> Expire(Guid sessionId);
	Task<Result> StopByUser(Guid sessionId, Guid userId);
}
