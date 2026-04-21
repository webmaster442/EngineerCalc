//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Infrastructure;

internal interface IRequestHandler<TRequest, TResponse> where TRequest : class, IRequest
{
    TResponse Handle(TRequest request);
}
