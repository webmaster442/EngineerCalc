//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Infrastructure;

internal interface IMediator
{
    void Send<TMessage>(TMessage message) where TMessage: class;

    TReply Send<TMessage, TReply>(TMessage message)
        where TMessage : class, IRequest
        where TReply : class;
}
