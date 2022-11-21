using AutoMapper;
using WebApplication1.Abstraction.Models;

namespace WebApplication1.Implementation.Helpers.Extensions;

public static class DateTimeExtensions
{
    public static DateTime TruncateHours(this DateTime source)
    {
        return new DateTime(
            year: source.Year,
            month: source.Month,
            day: source.Day);
    }
    
    public static DateTime TruncateMinutes(this DateTime source)
    {
        return new DateTime(
            year: source.Year,
            month: source.Month,
            day: source.Day,
            hour: source.Hour,
            minute: 0,
            second: 0);
    }
    
    public static DateTime TruncateSeconds(this DateTime source)
    {
        return new DateTime(
            year: source.Year,
            month: source.Month,
            day: source.Day,
            hour: source.Hour,
            minute: source.Minute,
            second: 0);
    }
}
