using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Application.Common.Exceptions;
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "<Pending>")]
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}
