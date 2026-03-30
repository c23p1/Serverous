using Application.Interfaces;
using Application.Interfaces.Identity;
using Application.Interfaces.Jobs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Jobs;
using Application.Services;
using Infrastructure.Identity;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Default")
			?? throw new KeyNotFoundException("Не удалось найти строку подключения к базе данных");

		services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
			dbContextOptions.UseNpgsql(connectionString));

		services.AddScoped<IServersRepository, ServersRepository>();
		services.AddScoped<ISessionsRepository, SessionsRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddTransient<IServersService, ServersService>();
		services.AddTransient<ISessionsService, SessionsService>();
		services.AddTransient<ISessionStateService, SessionStateService>();

		services.AddTransient<ISessionJobScheduler, HangfireSessionJobScheduler>();

		services.AddScoped<ICurrentUser, CurrentUser>();

		return services;
	}
}
