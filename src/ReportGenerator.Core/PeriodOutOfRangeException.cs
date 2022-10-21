namespace ReportGenerator.Core;

public class PeriodOutOfRangeException : Exception
{
    public PeriodOutOfRangeException() 
        : base("Trade period valid is out of range")
    { }
}