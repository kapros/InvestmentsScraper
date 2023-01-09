namespace InvestementsTracker.Comparers;

public class DateOnlyComparer : Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (d1, d2) => d1.DayNumber == d2.DayNumber,
        d => d.GetHashCode())
    {
    }
}