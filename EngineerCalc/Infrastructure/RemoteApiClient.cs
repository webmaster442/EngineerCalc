//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;

using EngineerCalc.Domain;
using EngineerCalc.Domain.Dto;

using Microsoft.Extensions.Configuration;

namespace EngineerCalc.Infrastructure;

internal sealed class RemoteApiClient : IDisposable, IRemoteApiClient
{
    private readonly IRemoteApiCache _cache;
    private readonly TimeProvider _timeProvider;
    private readonly HttpClient _client;
    private readonly string _exchangeRateUrl;

    public RemoteApiClient(IRemoteApiCache cache, TimeProvider timeProvider, IConfiguration configuration)
    {
        _cache = cache;
        _timeProvider = timeProvider;
        _client = new HttpClient();
        _exchangeRateUrl = configuration.GetSection("services")["exchangeRate"] ?? throw new InvalidOperationException("Exchange rate URL is not configured.");
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private const string OpenExchangeRatesKey = "OpenExchangeRates";

    public async ValueTask<OpenExchangeRatesResponse> GetOpenExchangeRatesAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGet(OpenExchangeRatesKey, out string jsonString))
        {
            OpenExchangeRatesResponse? cachedResponse = JsonSerializer.Deserialize<OpenExchangeRatesResponse>(jsonString, JsonSerializerOptions.Web);
            if (cachedResponse is not null)
            {
                return cachedResponse;
            }
        }

        var response = await _client.GetAsync(_exchangeRateUrl);
        response.EnsureSuccessStatusCode();

        jsonString = await response.Content.ReadAsStringAsync(cancellationToken);

        OpenExchangeRatesResponse result = JsonSerializer.Deserialize<OpenExchangeRatesResponse>(jsonString, JsonSerializerOptions.Web)
            ?? throw new InvalidOperationException("Failed to deserialize OpenExchangeRatesResponse.");

        _cache.Store("OpenExchangeRates", jsonString, DateTimeOffset.FromUnixTimeSeconds(result.TimeNextUpdateUnix).DateTime);

        return result;
    }
}
