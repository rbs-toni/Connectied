using MediatR;
using System;
using System.Linq;

namespace Connectied.Application.Contracts;
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
