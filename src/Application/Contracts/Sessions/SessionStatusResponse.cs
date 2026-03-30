using Domain.Enums;

namespace Application.Contracts.Sessions;

public record SessionStatusResponse
{
	public SessionStatus Status { get; init; }
}
