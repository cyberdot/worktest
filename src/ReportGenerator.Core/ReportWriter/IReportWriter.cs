namespace ReportGenerator.Core.ReportWriter;

public interface IReportWriter
{
    Task WriteAsync(IntraDayReport report);
}