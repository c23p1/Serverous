using Domain.Enums;
using OperatingSystem = Domain.Enums.OperatingSystem;

namespace Domain.Entities;

public class Server
{
	public Guid Id { get; set; }
	public OperatingSystem OperatingSystem { get; set; }
	public int RamGiB { get; set; }
	public int StorageGiB { get; set; }
	public int CpuCoreCount { get; set; }
	public ServerStatus Status { get; set; }
	public List<Session> Sessions { get; set; } = new List<Session>();
}
