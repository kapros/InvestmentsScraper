using System.Globalization;

namespace InvestementsTracker.Services
{
    public class DateFormatter : IFormatProvider
    {
        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(DateTimeFormatInfo))
            {
                return "yyyy-MM-dd";
            }
            return null;
        }
    }
}
