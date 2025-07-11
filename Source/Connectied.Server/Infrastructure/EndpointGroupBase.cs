using System;
using System.Linq;

namespace Connectied.Server.Infrastructure;
public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
