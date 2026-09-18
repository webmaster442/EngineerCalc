//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Domain.Dto;

namespace EngineerCalc.Domain;

internal interface IRemoteApiClient
{
    ValueTask<OpenExchangeRatesResponse> GetOpenExchangeRatesAsync(CancellationToken cancellationToken);
}