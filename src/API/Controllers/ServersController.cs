using API.Extensions;
using Application.Contracts;
using Application.Dto;
using Application.Interfaces.Services;
using Domain.Entities;
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
	public async Task<IActionResult> AddServer(ServerDto server)
	{
		var result = await _serversService.Add(server);
		return result.ToActionResult();
	}

	[HttpGet]
	public async Task<ActionResult<List<Server>>> GetAvailableAsync([FromQuery] ServerFilters filters) =>
		(await _serversService.GetAvailableAsync(filters)).ToActionResult();
}
