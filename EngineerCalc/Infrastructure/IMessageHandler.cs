//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Infrastructure;

internal interface IMessageHandler<in TMessage>  where TMessage : class
{
    void Handle(TMessage request);
}
