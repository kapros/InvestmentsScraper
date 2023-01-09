using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InvestementsTracker.Converters;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
            dateOnly => MakeDateTimeKind(dateOnly),
            dateTime => DateOnly.FromDateTime(dateTime))
    {
    }

    private static DateTime MakeDateTimeKind(DateOnly dt)
    {
        var date = dt.ToDateTime(TimeOnly.MinValue);
        return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
}
