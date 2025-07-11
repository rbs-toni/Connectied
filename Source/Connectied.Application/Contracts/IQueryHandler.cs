using MediatR;
using System;
using System.Linq;

namespace Connectied.Application.Contracts;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
       where TQuery : IQuery<TResponse>
{
}
