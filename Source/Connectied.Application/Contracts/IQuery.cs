using MediatR;
using System;
using System.Linq;

namespace Connectied.Application.Contracts;
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
