using Ardalis.Specification;
using Connectied.Domain;
using System;
using System.Linq;

namespace Connectied.Application.Repositories;
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
