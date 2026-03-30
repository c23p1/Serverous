using System.Security.Claims;
using System.Security.Authentication;
using Application.Interfaces.Identity;

namespace Infrastructure.Identity;

public class CurrentUser : ICurrentUser
{
	private Guid? _userId;

	public Guid UserId
	{
		get => _userId ?? throw new AuthenticationException("Сессия не инициализирована");
		private set => _userId = value;
	}

	public void Initialize(ClaimsPrincipal claimsPrincipal)
	{
		var userIdValue = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
			?? throw new AuthenticationException("При инициализации использован недействительный ClaimsPrincipal");

		UserId = Guid.Parse(userIdValue);
	}
}
