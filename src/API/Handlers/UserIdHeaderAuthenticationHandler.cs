using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Interfaces.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace API.Handlers;

public sealed class UserIdHeaderAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public const string SchemeName = "UserIdHeader";

	private readonly ICurrentUser _currentUser;

	public UserIdHeaderAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ICurrentUser currentUser) : base(options, logger, encoder)
	{
		_currentUser = currentUser;
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.TryGetValue("UserId", out var userIdHeader))
		{
			return Task.FromResult(AuthenticateResult.NoResult());
		}
		if (!Guid.TryParse(userIdHeader, out var userId))
		{
			return Task.FromResult(AuthenticateResult.Fail("Неверный формат идентификатора пользователя"));
		}

		var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
		var identity = new ClaimsIdentity(claims, SchemeName);
		var principal = new ClaimsPrincipal(identity);
		_currentUser.Initialize(principal);

		var ticket = new AuthenticationTicket(principal, SchemeName);
		return Task.FromResult(AuthenticateResult.Success(ticket));
	}
}
