using Ardalis.Specification;
using Connectied.Domain;
using System;
using System.Linq;

namespace Connectied.Application.Repositories;
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
