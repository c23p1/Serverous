using Application.Common;
using Application.Contracts.Servers;

namespace Application.Interfaces.Services;

public interface IServersService
{
	Task<GenericResult<List<ServerDetailResponse>>> GetAvailableAsync(ServerFilters filters);
	Task<GenericResult<ServerDetailResponse>> Add(AddServerRequest addServerRequest);
}
