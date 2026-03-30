using API.Extensions;
using Application.Contracts.Sessions;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("sessions")]
public class SessionsController : ControllerBase
{
	private readonly ISessionsService _sessionsService;

	public SessionsController(ISessionsService sessionsService)
	{
		_sessionsService = sessionsService;
	}

	[HttpPost("start")]
	public async Task<ActionResult<StartSessionResponse>> StartSession(Guid serverId) =>
		(await _sessionsService.Start(serverId)).ToActionResult();

	[HttpGet("{sessionId}/status")]
	public async Task<ActionResult<SessionStatusResponse>> GetSessionStatus(Guid sessionId) =>
		(await _sessionsService.GetStatusByIdAsync(sessionId)).ToActionResult();

	[HttpPost("{sessionId}/stop")]
	public async Task<IActionResult> StopSession(Guid sessionId) =>
		(await _sessionsService.Stop(sessionId)).ToActionResult();
}
