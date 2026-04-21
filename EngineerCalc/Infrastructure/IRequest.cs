//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Infrastructure;

internal interface IRequest;

internal interface IRequest<TRequest, TResponse> : IRequest where TRequest : class;
