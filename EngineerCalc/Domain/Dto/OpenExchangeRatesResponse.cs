//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EngineerCalc.Domain.Dto;

public sealed class OpenExchangeRatesResponse
{
    public enum ResultType
    {
        [JsonStringEnumMemberName("success")]
        Success,
        [JsonStringEnumMemberName("error")]
        Error
    }

    [JsonPropertyName("result")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ResultType Result { get; set; }

    [JsonPropertyName("time_last_update_unix")]
    public long TimeLastUpdateUnix { get; set; }

    [JsonPropertyName("time_last_update_utc")]
    public required string TimeLastUpdateUtc { get; set; }

    [JsonPropertyName("time_next_update_unix")]
    public long TimeNextUpdateUnix { get; set; }

    [JsonPropertyName("time_next_update_utc")]
    public required string TimeNextUpdateUtc { get; set; }

    [JsonPropertyName("time_eol_unix")]
    public long TimeEolUnix { get; set; }

    [JsonPropertyName("base_code")]
    public required string BaseCode { get; set; }

    [JsonPropertyName("rates")]
    public required Dictionary<string, double> Rates { get; set; }
}
