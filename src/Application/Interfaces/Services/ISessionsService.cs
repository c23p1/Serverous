using Application.Common;
using Application.Contracts.Sessions;

namespace Application.Interfaces.Services;

public interface ISessionsService
{
	Task<GenericResult<StartSessionResponse>> Start(Guid serverId);
	Task<GenericResult<SessionStatusResponse>> GetStatusByIdAsync(Guid sessionId);
	Task<Result> Stop(Guid sessionId);
}
