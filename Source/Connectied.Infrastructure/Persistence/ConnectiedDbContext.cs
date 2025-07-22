using Connectied.Domain.GuestLists;
using Connectied.Domain.Guests;
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
    public DbSet<GuestGroup> GuestGroups => Set<GuestGroup>();
    public DbSet<GuestList> GuestLists => Set<GuestList>();
    public DbSet<GuestListConfiguration> GuestListConfigurations => Set<GuestListConfiguration>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
