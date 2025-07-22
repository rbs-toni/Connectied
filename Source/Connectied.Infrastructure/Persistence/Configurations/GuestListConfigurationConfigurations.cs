using Connectied.Domain.GuestLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Connectied.Infrastructure.Persistence.Configurations;
public class GuestListConfigurationConfigurations : IEntityTypeConfiguration<GuestListConfiguration>
{
    public void Configure(EntityTypeBuilder<GuestListConfiguration> builder)
    {
        ConfigurePrimitiveCollection(builder.Property(x => x.Columns));
        ConfigurePrimitiveCollection(builder.Property(x => x.Groups));
        ConfigurePrimitiveCollection(builder.Property(x => x.IncludedGuests));
        ConfigurePrimitiveCollection(builder.Property(x => x.ExcludedGuests));
    }
    static void ConfigurePrimitiveCollection(PropertyBuilder<ICollection<string>?> propertyBuilder)
    {
        propertyBuilder.HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null),
            new ValueComparer<ICollection<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            )
        );
    }
}
