using OperatingSystem = Domain.Enums.OperatingSystem;
namespace Application.Contracts;

public class ServerFilters
{
	public OperatingSystem OperatingSystem { get; set; }
	public int MinRamGiB { get; set; }
	public int MaxRamGiB { get; set; }
	public int MinStorageGiB { get; set; }
	public int MaxStorageGiB { get; set; }
	public int MinCpuCoreCount { get; set; }
	public int MaxCpuCoreCount { get; set; }
}
