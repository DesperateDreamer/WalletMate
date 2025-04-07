namespace WalletMate.BLL.Shared;

public static class DateTimeConverter
{
    public static long ToUnixTimestamp(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
    
    public static DateTime FromUnixTimestamp(long unixTimestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
    }
}