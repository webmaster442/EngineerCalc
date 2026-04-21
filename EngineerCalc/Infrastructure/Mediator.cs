//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace EngineerCalc.Infrastructure;

internal sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Send<TMessage>(TMessage message)
        where TMessage : class
    {
        var handlers = _serviceProvider.GetRequiredService<IEnumerable<IMessageHandler<TMessage>>>();
        foreach (var handler in handlers)
        {
            handler.Handle(message);
        }
    }

    public TReply Send<TMessage, TReply>(TMessage message)
        where TMessage : class, IRequest
        where TReply : class
    {
        var handler =_serviceProvider.GetRequiredService<IRequestHandler<TMessage, TReply>>();
        return handler.Handle(message);
    }
}
