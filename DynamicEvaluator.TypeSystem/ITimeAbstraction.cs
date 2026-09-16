namespace DynamicEvaluator.TypeSystem;

public interface ITimeAbstraction
{
    DateTimeOffset GetUtcNow();

    ValueTask<DateTimeOffset> GetNetworkUtcNow();

    DateTimeOffset GetNow()
        => GetUtcNow().ToLocalTime();

    async ValueTask<DateTimeOffset> GetNetworkNow()
    {
        DateTimeOffset utcValue = await GetNetworkUtcNow();
        return utcValue.ToLocalTime();
    }
}
