using Infrastructure.DependencyInjection;
using Scalar.AspNetCore;

namespace API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Configuration.AddEnvironmentVariables();

		builder.Services.AddInfrastructure(builder.Configuration);

		builder.Services.AddOpenApi();

		builder.Services.AddControllers();

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.MapScalarApiReference("reference");
		}

		app.UseHttpsRedirection();

		app.MapControllers();

		app.Run();
	}
}
