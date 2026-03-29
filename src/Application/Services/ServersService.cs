using Application.Common;
using Application.Contracts.Servers;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ServersService : IServersService
{
	private readonly IServersRepository _serversRepository;
	private readonly IUnitOfWork _unitOfWork;

	public ServersService(IServersRepository serversRepository, IUnitOfWork unitOfWork)
	{
		_serversRepository = serversRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<GenericResult<List<Server>>> GetAvailableAsync(ServerFilters filters) =>
		GenericResult<List<Server>>.Success(await _serversRepository.GetAvailableAsync(filters));

	public async Task<GenericResult<AddServerResponse>> Add(AddServerRequest addServerRequest)
	{
		var server = new Server
		{
			OperatingSystem = addServerRequest.OperatingSystem,
			RamGiB = addServerRequest.RamGiB,
			StorageGiB = addServerRequest.StorageGiB,
			CpuCoreCount = addServerRequest.CpuCoreCount,
			Status = ServerStatus.Stopped
		};
		_serversRepository.Add(server);
		await _unitOfWork.SaveChangesAsync();

		var addServerResponse = new AddServerResponse
		{
			Id = server.Id,
			OperatingSystem = server.OperatingSystem,
			RamGiB = server.RamGiB,
			StorageGiB = server.StorageGiB,
			CpuCoreCount = server.CpuCoreCount
		};
		return GenericResult<AddServerResponse>.Success(addServerResponse);
	}

	public async Task<Result> StartServerById(Guid serverId)
	{
		var server = await _serversRepository.GetByIdAsync(serverId);
		if (server is null)
		{
			return Result.Failure(Error.EntityNotFound(nameof(Server), serverId.ToString()));
		}
		server.Status = ServerStatus.Starting;
		_serversRepository.Update(server);
		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}

	public async Task<Result> StopServerById(Guid serverId)
	{
		var server = await _serversRepository.GetByIdAsync(serverId);
		if (server is null)
		{
			return Result.Failure(Error.EntityNotFound(nameof(Server), serverId.ToString()));
		}
		server.Status = ServerStatus.Stopped;
		_serversRepository.Update(server);
		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
