using System.Text.Json.Serialization;
using API.Extensions;
using API.Handlers;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;

namespace API;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Configuration.AddEnvironmentVariables();

		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddHangfireServices(builder.Configuration);

		builder.Services.AddAuthentication(UserIdHeaderAuthenticationHandler.SchemeName)
			.AddScheme<AuthenticationSchemeOptions, UserIdHeaderAuthenticationHandler>(UserIdHeaderAuthenticationHandler.SchemeName, null);

		builder.Services.AddAuthorization();

		builder.Services.AddOpenApi();

		builder.Services.AddControllers()
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

		var app = builder.Build();

		await app.Services.InitializeDatabaseAsync();

		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.MapScalarApiReference("reference");
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
