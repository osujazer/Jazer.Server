using System.Reflection;
using Mapster;

namespace Jazer.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Entities.User, Models.User>
            .NewConfig()
            .Map(dest => dest.Statistics, src => src.UserStatistics.Adapt<Models.UserStatistics>())
            .Map(dest => dest.PeakRank, src => src.UserPeakRank.Adapt<Models.UserPeakRank?>());
        
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        return services;
    }
}