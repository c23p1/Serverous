using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<Server> Servers { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<Session> Sessions { get; set; }
}
