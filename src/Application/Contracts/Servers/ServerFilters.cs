using OperatingSystem = Domain.Enums.OperatingSystem;

namespace Application.Contracts.Servers;

public record ServerFilters
{
	public OperatingSystem? OperatingSystem { get; init; }
	public int MinRamGiB { get; init; }
	public int MaxRamGiB { get; init; }
	public int MinStorageGiB { get; init; }
	public int MaxStorageGiB { get; init; }
	public int MinCpuCoreCount { get; init; }
	public int MaxCpuCoreCount { get; init; }
}
