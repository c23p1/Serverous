using OperatingSystem = Domain.Enums.OperatingSystem;

namespace Application.Dto;

public class ServerDto
{
	public OperatingSystem OperatingSystem { get; set; }
	public int RamGiB { get; set; }
	public int StorageGiB { get; set; }
	public int CpuCoreCount { get; set; }
}
