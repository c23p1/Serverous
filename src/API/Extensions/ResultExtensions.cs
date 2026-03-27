using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ResultExtensions
{
	public static ActionResult<T> ToActionResult<T>(this GenericResult<T> result)
	{
		if (result.IsSuccess)
		{
			return new OkObjectResult(result.Value);
		}

		var status = MapStatus(result.Error);
		return new ObjectResult(new ProblemDetails
		{
			Status = status,
			Title = result.Error!.Title,
			Detail = result.Error!.Message
		})
		{
			StatusCode = status
		};
	}

	public static IActionResult ToActionResult(this Result result)
	{
		if (result.IsSuccess)
		{
			return new NoContentResult();
		}

		var status = MapStatus(result.Error);
		return new ObjectResult(new ProblemDetails
		{
			Status = status,
			Title = result.Error!.Title,
			Detail = result.Error!.Message
		})
		{
			StatusCode = status
		};
	}

	private static int MapStatus(Error? error) => error?.ErrorType switch
	{
		ErrorType.Validation => StatusCodes.Status400BadRequest,
		ErrorType.InvalidOperation => StatusCodes.Status400BadRequest,
		ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
		ErrorType.NotFound => StatusCodes.Status404NotFound,
		_ => StatusCodes.Status500InternalServerError
	};
}
