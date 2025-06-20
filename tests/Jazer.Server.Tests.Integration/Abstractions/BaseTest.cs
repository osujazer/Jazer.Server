using Jazer.Server.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Jazer.Server.Tests.Integration.Abstractions;


public abstract class BaseTest(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>
{
    protected readonly IServiceScope Scope = factory.Services.CreateScope();

    protected JazerDbContext DbContext => Scope.ServiceProvider.GetService<JazerDbContext>()!;
}