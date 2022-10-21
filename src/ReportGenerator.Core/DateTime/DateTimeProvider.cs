namespace ReportGenerator.Core.DateTime;

public class DateTimeProvider : IDateTimeProvider
{
    private static readonly TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

    public System.DateTime GetCurrentUkDateTime()
    {
        var currentLocalDateTime = System.DateTime.Now;
        return TimeZoneInfo.ConvertTime(currentLocalDateTime, TimeZoneInfo.Local, ukTimeZone);
    }
}