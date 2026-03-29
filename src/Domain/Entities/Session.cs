namespace Domain.Entities;

public class Session
{
	public Guid Id { get; set; }
	public Guid ServerId { get; set; }
	public Guid CustomerId { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public bool IsActive { get; set; }
}
