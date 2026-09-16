namespace DynamicEvaluator.TypeSystem;

public interface ITimeAbstraction
{
    DateTimeOffset GetUtcNow();

    ValueTask<DateTimeOffset> GetNetworkUtcNow();

    DateTimeOffset GetNow()
    {
        TimeSpan offset = GetLocalTimeZone().GetUtcOffset(GetUtcNow());
        long localTicks = GetUtcNow().Ticks + offset.Ticks;
        return new DateTimeOffset(localTicks, offset);
    }

    async ValueTask<DateTimeOffset> GetNetworkNow()
    {
        DateTimeOffset utcValue = await GetNetworkUtcNow();
        TimeSpan offset = GetLocalTimeZone().GetUtcOffset(utcValue);
        long localTicks = utcValue.Ticks + offset.Ticks;
        return new DateTimeOffset(localTicks, offset);
    }

    TimeZoneInfo GetLocalTimeZone()
        => TimeZoneInfo.Local;
}
