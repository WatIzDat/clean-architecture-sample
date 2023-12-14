using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Messaging
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
        //Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
        //Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
