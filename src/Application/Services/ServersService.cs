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

	public async Task<GenericResult<List<ServerDetailResponse>>> GetAvailableAsync(ServerFilters filters) =>
		GenericResult<List<ServerDetailResponse>>.Success((await _serversRepository.GetAvailableAsync(filters))
			.ConvertAll(s =>
				new ServerDetailResponse
				{
					Id = s.Id,
					OperatingSystem = s.OperatingSystem,
					RamGiB = s.RamGiB,
					StorageGiB = s.StorageGiB,
					CpuCoreCount = s.CpuCoreCount
				}));

	public async Task<GenericResult<ServerDetailResponse>> Add(AddServerRequest addServerRequest)
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

		var serverDetailResponse = new ServerDetailResponse
		{
			Id = server.Id,
			OperatingSystem = server.OperatingSystem,
			RamGiB = server.RamGiB,
			StorageGiB = server.StorageGiB,
			CpuCoreCount = server.CpuCoreCount
		};
		return GenericResult<ServerDetailResponse>.Success(serverDetailResponse);
	}
}
