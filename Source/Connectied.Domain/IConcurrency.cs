using System;
using System.Linq;

namespace Connectied.Domain;
public interface IConcurrency
{
    byte[] Version { get; }
}
