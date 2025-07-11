using MediatR;
using System;
using System.Linq;

namespace Connectied.Application.Contracts;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
{
}
