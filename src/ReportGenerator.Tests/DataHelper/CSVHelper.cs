using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ReportGenerator.Core;
using ReportGenerator.Core.ReportWriter;

namespace ReportGenerator.Tests.DataHelper;

public static class CSVHelper
{
    public static IEnumerable<TradePeriod> ReadCsvReport(string filePath)
    {
        var records = new List<TradePeriod>();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found!!!");
        }
        
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<TradePeriodMap>();
          
            records = csv.GetRecords<TradePeriod>().ToList();
        }

        return records;
    }
}