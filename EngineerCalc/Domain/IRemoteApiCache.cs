//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Domain;

internal interface IRemoteApiCache
{
    void Store(string key, string value, DateTime expiryDate);
    void Store(string key, string value, TimeSpan timeToLive);
    bool TryGet(string key, out string value);
}
