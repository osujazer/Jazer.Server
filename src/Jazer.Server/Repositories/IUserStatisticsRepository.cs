namespace Jazer.Server.Repositories;

public interface IUserStatisticsRepository
{
    Task Add(int userId, CancellationToken cancellationToken = default);
}