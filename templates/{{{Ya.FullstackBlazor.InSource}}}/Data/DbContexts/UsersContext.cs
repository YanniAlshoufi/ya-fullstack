using Microsoft.EntityFrameworkCore;
using Shared.Data.Models;

namespace Data.DbContexts;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}