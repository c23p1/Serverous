using API.Extensions;
using Application.Contracts.Servers;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("servers")]
public class ServersController : ControllerBase
{
	private readonly IServersService _serversService;

	public ServersController(IServersService serversService)
	{
		_serversService = serversService;
	}

	[HttpPost]
	public async Task<ActionResult<ServerDetailResponse>> AddServer(AddServerRequest addServerRequest)
	{
		var result = await _serversService.Add(addServerRequest);
		return result.ToActionResult();
	}

	[HttpGet("available")]
	public async Task<ActionResult<List<ServerDetailResponse>>> GetAvailableAsync([FromQuery] ServerFilters filters) =>
		(await _serversService.GetAvailableAsync(filters)).ToActionResult();
}
