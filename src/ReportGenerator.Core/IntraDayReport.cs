namespace ReportGenerator.Core;

public class IntraDayReport
{
    private readonly IDictionary<int, TradePeriod> positions = new Dictionary<int, TradePeriod>();

    public IntraDayReport(System.DateTime dateTime)
    {
        FileName = $"PowerPosition_{dateTime.ToString("yyyyMMdd_HHmm")}.csv";
    }

    public void Aggregate(TradePeriod tradePeriod)
    {
        if (positions.TryGetValue(tradePeriod.Period, out var value))
        {
            value.UpdateVolume(tradePeriod.Volume);
        }
        else
        {
            positions[tradePeriod.Period] = tradePeriod;
        }
    }

    public IEnumerable<TradePeriod> AggregatePositions => positions.Values;
    public string FileName { get; }
}