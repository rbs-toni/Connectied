using Connectied.Server.Infrastructure;

namespace Connectied.Server.Endpoints;

public class Weddings : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this);
    }
}

