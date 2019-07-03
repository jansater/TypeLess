using System;
using System.Globalization;

namespace TypeLess.Net
{
    public static class DateHelper
    {
        public static void GetStartAndEndOfWeek(int year, int week, out DateTime startOfWeek, out DateTime endOfWeek, CalendarWeekRule cwr, DayOfWeek firstDayOfWeek)
        {
            // find the first week. 
            //CalendarWeekRule cwr = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
            //DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime firstdayofyear = new DateTime(year, 1, 1);
            int offset = 0;
            if (firstdayofyear.DayOfWeek != firstDayOfWeek)
            {
                // find first first day. 
                if (cwr == CalendarWeekRule.FirstFourDayWeek)
                {
                    DateTime firstFullWeekStart = firstdayofyear;
                    while (firstFullWeekStart.DayOfWeek != firstDayOfWeek)
                        firstFullWeekStart = firstFullWeekStart.AddDays(1);
                    if (firstFullWeekStart.Subtract(firstdayofyear).Days >= 4)
                        offset = -1;
                }
                if (cwr == CalendarWeekRule.FirstDay)
                    offset = -1;
            }
            startOfWeek = firstdayofyear.AddDays(7 * (week + offset));
            while (startOfWeek != firstdayofyear && startOfWeek.DayOfWeek != firstDayOfWeek)
                startOfWeek = startOfWeek.AddDays(-1);
            endOfWeek = startOfWeek;
            do
            {
                endOfWeek = endOfWeek.AddDays(1);
            } while (endOfWeek < new DateTime(year + 1, 1, 1).AddDays(-1) && endOfWeek.AddDays(1).DayOfWeek != firstDayOfWeek);
        }

        public static int GetNumberOfDaysInMonthExcludingWeekday(DateTime date, DayOfWeek day)
        {
            return GetNumberOfDaysInMonth(date) - GetNumberOfWeekdaysInMonth(date, day);
        }

        public static int GetNumberOfDaysPastExcludingWeekday(DateTime date, DayOfWeek day)
        {
            return date.Day - GetNumberOfWeekdaysInDaysPast(date, day);
        }

        public static int GetNumberOfDaysInMonth(DateTime date)
        {
            var calendar = new GregorianCalendar();
            return calendar.GetDaysInMonth(date.Year, date.Month);
        }

        /// <summary>
        /// Gets the number of the given weekday in the month of the given date
        /// </summary>
        public static int GetNumberOfWeekdaysInMonth(DateTime date, DayOfWeek day)
        {
            var nWeekdays = 0;
            var daysInMonth = GetNumberOfDaysInMonth(date);

            var tempDate = GetStartOfMonth(date);
            for (int i = 1; i <= daysInMonth; i++, tempDate = tempDate.AddDays(1))
            {
                if (tempDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    nWeekdays++;
                }
            }
            return nWeekdays;
        }

        /// <summary>
        /// Gets the number of the given weekday in the days past in the month of the given date
        /// </summary>
        public static int GetNumberOfWeekdaysInDaysPast(DateTime date, DayOfWeek day)
        {
            var nWeekdays = 0;
            var calendar = new GregorianCalendar();
            var daysInMonth = calendar.GetDaysInMonth(date.Year, date.Month);

            var tempDate = GetStartOfMonth(date);
            for (int i = 1; i <= date.Day; i++, tempDate = tempDate.AddDays(1))
            {
                if (tempDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    nWeekdays++;
                }
            }
            return nWeekdays;
        }

        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime GetStartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetEndOfMonth(DateTime date)
        {
            DateTime d = new DateTime(date.Year, date.Month, 1);
            d = d.AddMonths(1);
            d = d.AddSeconds(-1);
            return d;
        }

        public static DateTime GetStartOfMonth(int monthOffsetFromCurrentMonth)
        {
            DateTime d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return d.AddMonths(monthOffsetFromCurrentMonth);
        }

        public static DateTime GetEndOfMonth(int monthOffsetFromCurrentMonth)
        {
            DateTime d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            d = d.AddMonths(1);
            d.AddSeconds(-1);
            return d.AddMonths(monthOffsetFromCurrentMonth);
        }

        /// <summary>
        /// Returns true if date occurs within days before or on fromDate
        /// </summary>
        public static bool OccursWithinDaysBefore(DateTime date, int days, DateTime fromDate)
        {
            if (days < 0)
            {
                throw new ArgumentException("days must be > 0");
            }

            date = date.Date;
            fromDate = fromDate.Date;

            return date >= fromDate.AddDays(-days) && date <= fromDate;
        }

        /// <summary>
        /// Returns true if date occurs within days after or on fromDate
        /// </summary>
        public static bool OccursWithinDaysAfter(DateTime date, int days, DateTime fromDate)
        {
            if (days < 0)
            {
                throw new ArgumentException("days must be > 0");
            }

            date = date.Date;
            fromDate = fromDate.Date;

            return date >= fromDate && date <= fromDate.AddDays(days);
        }

        /// <summary>
        /// Returns the number of days in a period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDaysInPeriod(DateTime startDate, DateTime endDate)
        {
            TimeSpan span = TimeSpan.FromTicks(endDate.Ticks - startDate.Ticks);
            return span.Days;
        }

        public static bool IsInSameMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.Year == endDate.Year && startDate.Month == endDate.Month;
        }

        public static bool IsInSameYear(DateTime startDate, DateTime endDate)
        {
            return startDate.Year == endDate.Year;
        }


        public static DateTime GetStartOfYear(DateTime now)
        {
            return new DateTime(now.Year, 1, 1);
        }

        public static DateTime GetEndOfYear(DateTime now)
        {
            DateTime nextYear = new DateTime(now.Year + 1, 1, 1);
            return nextYear.AddSeconds(-1);
        }

        public static DateTime GetQuarterStartDate(DateTime date)
        {
            var quarter = GetQuarter(date);
            return new DateTime(date.Year, 3 * quarter - 2, 1);
        }

        public static DateTime GetQuarterEndDate(DateTime date)
        {
            var quarter = GetQuarter(date);
            var d = new DateTime(date.Year, 3 * quarter, 1);
            return GetEndOfMonth(d);
        }

        public static DateTime GetQuarterStartDate(int year, int quarter)
        {
            return new DateTime(year, 3 * quarter - 2, 1);
        }

        public static DateTime GetQuarterEndDate(int year, int quarter)
        {
            var d = new DateTime(year, 3 * quarter, 1);
            return GetEndOfMonth(d);
        }

        public static int GetQuarter(DateTime date)
        {
            return (int)(Math.Floor((((double)date.Month - 1) / 3)) + 1);
        }

        public static int GetNextQuarter(DateTime date)
        {
            var nextQuarter = GetQuarter(date) + 1;
            nextQuarter = nextQuarter % 5;
            if (nextQuarter == 0)
            {
                nextQuarter++;
            }
            return nextQuarter;
        }

        public static int GetPreviousQuarter(DateTime date)
        {
            var prevQuarter = GetQuarter(date) - 1;
            prevQuarter = prevQuarter % 5;
            if (prevQuarter == 0)
            {
                return 4;
            }
            return prevQuarter;
        }
    }
}
