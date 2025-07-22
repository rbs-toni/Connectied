using System;
using System.Linq;

namespace Connectied.Domain;
[AttributeUsage(AttributeTargets.Property)]
public sealed class ColumnViewableAttribute : Attribute
{
}
