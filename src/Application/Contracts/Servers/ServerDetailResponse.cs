using OperatingSystem = Domain.Enums.OperatingSystem;

namespace Application.Contracts.Servers;

public record ServerDetailResponse
{
	public Guid Id { get; init; }
	public OperatingSystem OperatingSystem { get; init; }
	public int RamGiB { get; init; }
	public int StorageGiB { get; init; }
	public int CpuCoreCount { get; init; }
}
