using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Connectied.Server.Infrastructure;
public static partial class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);

        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupType = group.GetType();
        var groupName = groupType.Name; // e.g., "UserProfilesGroup"

        // (1) URL path: "user-profiles" (hyphenated, lowercase)
        var urlPath = HyphenateRegex().Replace(groupName, "-$1")
                          .ToLowerInvariant();

        // (2) Swagger tag: "User Profiles" (human-friendly)
        var swaggerTag = HumanFriendlyNameRegex().Replace(groupName, "$1 $2");

        return app
            .MapGroup($"/api/{urlPath}")
            .WithTags(swaggerTag);
    }

    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex HyphenateRegex();
    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex HumanFriendlyNameRegex();
}
