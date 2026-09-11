namespace EngineerCalc.Domain;

internal interface IRemoteApiCache
{
    void Store(string key, string value, DateTime expiryDate);
    void Store(string key, string value, TimeSpan timeToLive);
    bool TryGet(string key, out string value);
}
