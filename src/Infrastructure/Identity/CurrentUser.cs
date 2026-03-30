using System.Security.Claims;
using System.Security.Authentication;
using Application.Interfaces.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity;

public class CurrentUser : ICurrentUser
{
	private Guid? _userId;
	private readonly ILogger<CurrentUser> _logger;

	public CurrentUser(ILogger<CurrentUser> logger)
	{
		_logger = logger;
	}

	public Guid UserId
	{
		get
		{
			if (_userId is null)
			{
				_logger.LogWarning("Сессия не инициализирована");
				throw new AuthenticationException("Сессия не инициализирована");
			}

			return _userId.Value;
		}
		private set => _userId = value;
	}

	public void Initialize(ClaimsPrincipal claimsPrincipal)
	{
		var userIdValue = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (string.IsNullOrWhiteSpace(userIdValue))
		{
			_logger.LogWarning("Ошибка инициализации: при инициализации использован недействительный ClaimsPrincipal");
			throw new AuthenticationException("При инициализации использован недействительный ClaimsPrincipal");
		}

		if (!Guid.TryParse(userIdValue, out var userId))
		{
			_logger.LogWarning("Ошибка инициализации сессии: некорректный формат UserId ({UserId})", userIdValue);
			throw new AuthenticationException("При инициализации использован недействительный ClaimsPrincipal");
		}

		UserId = userId;
		_logger.LogInformation("Текущий пользователь инициализирован с UserId {UserId}", UserId);
	}
}
