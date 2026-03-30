using Domain.Enums;

namespace Domain.Entities;

public class Session
{
	public Guid Id { get; set; }
	public Guid ServerId { get; set; }
	public Guid UserId { get; set; }
	public DateTime? StartTime { get; set; }
	public DateTime? EndTime { get; set; }
	public bool IsActive { get; set; }
	public SessionStatus Status { get; set; }
	public DateTime CreatedAt { get; set; }
	public Server? Server { get; set; }
}
