using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using EngineerCalc.Domain;
using EngineerCalc.Domain.Dto;

namespace EngineerCalc.Infrastructure;


internal sealed class RemoteApiClient : IDisposable
{
    private readonly IRemoteApiCache _cache;
    private readonly TimeProvider _timeProvider;
    private readonly HttpClient _client;

    public RemoteApiClient(IRemoteApiCache cache, TimeProvider timeProvider)
    {
        _cache = cache;
        _timeProvider = timeProvider;
        _client = new HttpClient();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private const string OpenExchangeRatesKey = "OpenExchangeRates";

    /*public async ValueTask<OpenExchangeRatesResponse> GetOpenExchangeRatesAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGet(OpenExchangeRatesKey, out string jsonString))
        {
            var cachedResponse = JsonSerializer.Deserialize<OpenExchangeRatesResponse>(jsonString, JsonSerializerOptions.Web);
            if (cachedResponse is not null)
            {
                return cachedResponse;
            }
        }
    }*/
}
