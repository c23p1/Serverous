using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;

namespace API.Extensions;

public static class HangfireExtensions
{
	public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Default")
			?? throw new KeyNotFoundException("Не удалось найти строку подключения к базе данных");

		services.AddHangfire(config => config
			.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
			.UseRecommendedSerializerSettings()
			.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString)));

		services.AddHangfireServer();
		return services;
	}
}
