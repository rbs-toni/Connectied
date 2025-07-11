﻿using Ardalis.GuardClauses;
using System;
using System.Linq;
using System.Reflection;

namespace Connectied.Server.Infrastructure;
public static class MethodInfoExtensions
{
    public static void AnonymousMethod(this IGuardClause guardClause, Delegate input)
    {
        if (input.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");
    }
    public static bool IsAnonymous(this MethodInfo method)
    {
        var invalidChars = new[] { '<', '>' };
        return method.Name.Any(invalidChars.Contains);
    }
}
