using Connectied.Domain.GuestList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Connectied.Infrastructure.Persistence;
public class ConnectiedDbContext : DbContext
{
    public ConnectiedDbContext(DbContextOptions<ConnectiedDbContext> options) : base(options)
    {
    }

    public DbSet<Guest> Guests => Set<Guest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
