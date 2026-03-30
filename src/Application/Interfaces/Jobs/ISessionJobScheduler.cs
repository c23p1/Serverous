namespace Application.Interfaces.Jobs;

public interface ISessionJobScheduler
{
	void ScheduleActivation(Guid sessionId, TimeSpan delay);
	void ScheduleExpiration(Guid sessionId, TimeSpan delay);
}
