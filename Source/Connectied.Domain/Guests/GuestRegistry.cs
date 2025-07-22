using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Connectied.Domain.Guests;
[Table("GuestRegistries", Schema = "Connectied")]
public class GuestRegistry : BaseEntity
{
    public string? EventName { get; set; }
    public GuestRegistryType Type { get; set; }
    public int Quantity { get; set; }
}
