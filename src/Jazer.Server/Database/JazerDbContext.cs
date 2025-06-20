using Jazer.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Database;

public class JazerDbContext(DbContextOptions<JazerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
}