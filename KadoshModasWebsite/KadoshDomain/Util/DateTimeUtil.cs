namespace KadoshDomain.Util
{
    // TODO Check if this place is actually the best for this class. Should we move it to another commom project?
    internal static class DateTimeUtil
    {
        /// <summary>
        /// Returns the week range from a specific date.
        /// </summary>
        /// <param name="date">Date to calculate week range.</param>
        /// <returns>Tuple with past sunday at the very begining of the day and next saturday at the very end of the day from given date. In this case week starts on sunday and ends on saturdary.</returns>
        internal static (DateTime pastSunday, DateTime nextSaturday) GetWeekRangeFromDate(DateTime date)
        {
            DateTime pastSunday = new(
                year: date.Year,
                month: date.Month,
                day: date.Day - (int) date.DayOfWeek,
                hour: 0,
                minute: 0,
                second: 0);

            DateTime nextSaturday = new(
                year: date.Year, 
                month: date.Month,
                day: date.Day + ((int) DayOfWeek.Saturday -  (int)date.DayOfWeek),
                hour: 23,
                minute: 59,
                second: 59);
            return (pastSunday, nextSaturday);
        }
    }
}
