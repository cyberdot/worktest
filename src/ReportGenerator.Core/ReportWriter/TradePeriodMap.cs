using System.Globalization;
using CsvHelper.Configuration;

namespace ReportGenerator.Core.ReportWriter;

public class TradePeriodMap : ClassMap<TradePeriod>
{
    public TradePeriodMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(p => p.LocalTime).Index(0).Name("Local Time");
        Map(p => p.Volume).Index(1).Name("Volume");
        Map(p => p.Period).Ignore();
    }
}