using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Default")
			?? throw new KeyNotFoundException("Не удалось найти строку подключения к базе данных");

		services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
			dbContextOptions.UseNpgsql(connectionString));

		var serviceProvider = services.BuildServiceProvider();
		using (var scope = serviceProvider.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.Database.EnsureCreated();
		}

		services.AddScoped<IServersRepository, ServersRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddTransient<IServersService, ServersService>();

		return services;
	}
}
