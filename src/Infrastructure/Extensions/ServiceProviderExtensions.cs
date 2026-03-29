using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
	public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();

		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		await dbContext.Database.EnsureCreatedAsync();
		await SeedAsync(dbContext);
	}

	private static async Task SeedAsync(ApplicationDbContext dbContext)
	{
		if (await dbContext.Set<User>().AnyAsync())
		{
			return;
		}

		dbContext.Set<User>().AddRange
		(
			new User
			{
				Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
				Name = "Евгений"
			},
			new User
			{
				Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
				Name = "Сергей"
			}
		);

		await dbContext.SaveChangesAsync();
	}
}
