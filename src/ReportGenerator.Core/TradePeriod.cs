namespace ReportGenerator.Core;

public class TradePeriod
{
    public int Period { get; }

    public double Volume { get; private set; }
    
    public string LocalTime { get; }

    public TradePeriod(int period, double volume)
    {
        AssertPeriodIsValid(period);
        
        Period = period;
        LocalTime = GetPeriodLocalTime(period);
        Volume = volume;
    }

    public void UpdateVolume(double volume) => Volume += volume;

    private static void AssertPeriodIsValid(int period)
    {
        if (period is < 1 or > 24)
        {
            throw new PeriodOutOfRangeException();
        }
    }

    private static string GetPeriodLocalTime(int period)
    {
        if (period == 1)
        {
            return "23:00";
        }

        var timeValue = period - 2;

        return timeValue >= 10 ? $"{timeValue}:00" : $"0{timeValue}:00";
    }
}