using Jazer.Server.Database;
using Jazer.Server.Entities;

namespace Jazer.Server.Repositories;

internal sealed class UserStatisticsRepository(JazerDbContext dbContext) : IUserStatisticsRepository
{
    public async Task Add(int userId, CancellationToken cancellationToken = default)
    {
        var userStatistics = new UserStatistics
        {
            UserId = userId,
            JazerPoints = 0,
            RankedScore = 0,
            TotalScore = 0,
            AverageAccuracy = 0,
            PlayTimeSeconds = 0,
            PlayCount = 0,
            MaxCombo = 0,
            TotalHits = 0,
        };
        
        dbContext.UserStatistics.Add(userStatistics);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}