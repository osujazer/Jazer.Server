using Jazer.Server.Entities;

namespace Jazer.Server.Repositories;

public interface IUserRepository
{
    Task<int> Add(string username, string email, string hashedPassword, CancellationToken cancellationToken = default);

    Task<bool> UsernameExists(string username, CancellationToken cancellationToken = default);
    
    Task<bool> EmailExists(string email, CancellationToken cancellationToken = default);
    
    Task<User?> FindByUsername(string username, CancellationToken cancellationToken = default);
    
    Task<User?> FindByEmail(string email, CancellationToken cancellationToken = default);
    
    Task<User?> FindById(int id, CancellationToken cancellationToken = default);
}