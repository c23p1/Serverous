using Application.Interfaces.Jobs;
using Application.Interfaces.Services;
using Hangfire;

namespace Infrastructure.Jobs;

public class HangfireSessionJobScheduler : ISessionJobScheduler
{
	private readonly IBackgroundJobClient _backgroundJobClient;

	public HangfireSessionJobScheduler(IBackgroundJobClient backgroundJobClient)
	{
		_backgroundJobClient = backgroundJobClient;
	}

	public void ScheduleActivation(Guid sessionId, TimeSpan delay)
	{
		_backgroundJobClient.Schedule<ISessionStateService>(service => service.Activate(sessionId), delay);
	}

	public void ScheduleExpiration(Guid sessionId, TimeSpan delay)
	{
		_backgroundJobClient.Schedule<ISessionStateService>(service => service.Expire(sessionId), delay);
	}
}
