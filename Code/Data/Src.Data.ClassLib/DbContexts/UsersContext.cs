using Microsoft.EntityFrameworkCore;
using Src.Data.ClassLib.Models;

namespace Src.Data.ClassLib.DbContexts;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var user = modelBuilder.Entity<User>();

        user.HasKey(u => u.Id);
        
        user
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        user
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);
    }
}