using System.Security.Claims;

namespace Application.Interfaces.Identity;

public interface ICurrentUser
{
	Guid UserId { get; }
	void Initialize(ClaimsPrincipal claimsPrincipal);
}
