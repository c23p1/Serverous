using Application.Common;
using Application.Contracts.Servers;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IServersService
{
	Task<GenericResult<List<Server>>> GetAvailableAsync(ServerFilters filters);
	Task<GenericResult<AddServerResponse>> Add(AddServerRequest addServerRequest);
}
