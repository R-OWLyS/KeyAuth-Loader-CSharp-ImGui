namespace KeyAuth.Utility;

public static class TimeClock
{
    public static DateTime UnixTimeToDateTime(long unixtime)
    {
        try
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixtime).LocalDateTime;
        }
        catch
        {
            return DateTime.MaxValue;
        }
    }
}