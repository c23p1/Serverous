using Application.Common;
using Application.Contracts;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IServersService
{
	Task<GenericResult<List<Server>>> GetAvailableAsync(ServerFilters filters);
	Task<Result> Add(ServerDto server);
	Task<Result> StartServerById(Guid serverId);
	Task<Result> StopServerById(Guid serverId);
}
