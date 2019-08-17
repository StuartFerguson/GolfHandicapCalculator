namespace ManagementAPI.BusinessLogic.Manager
{
    using System;

    public static class ISOWeek
    {
        // If the week number thus obtained equals 0, it means that the given date belongs to the preceding (week-based) year.
        // If a week number of 53 is obtained, one must check that the date is not actually in week 1 of the following year.
        public static Int32 GetWeekNumber(DateTime date)
        {
            return (date.DayOfYear - ISOWeek.GetWeekday(date.DayOfWeek) + 10) / 7;
        }

        // Day of week in ISO is represented by an integer from 1 through 7, beginning with Monday and ending with Sunday.
        // This matches the underlying values of the DayOfWeek enum, except for Sunday, which needs to be converted.
        private static Int32 GetWeekday(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 7 : (Int32)dayOfWeek;
        }
    }
}