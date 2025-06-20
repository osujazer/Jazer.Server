namespace Jazer.Server.Repositories;

public interface IUserRepository
{
    Task<int> Add(string username, string email, string hashedPassword, CancellationToken cancellationToken = default);

    Task<bool> UsernameExists(string username, CancellationToken cancellationToken = default);
    
    Task<bool> EmailExists(string email, CancellationToken cancellationToken = default);
}