using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ti.Poc.Calendar.Test
{
    [TestClass]
    public class WorkdayCalendarTest
    {
        WorkdayCalendar workdayCalendar;
        [TestInitialize]
        public void Setup()
        {
            workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkDayStartAndStop(new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0));
        }
        [TestMethod]
        public void WorkdayIncrementByOneDayFromMonday()
        {

            var startDate = new DateTime(2015, 01, 01, 8, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Monday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 1);
            Assert.AreEqual(startDate.AddHours(8).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void WorkdayIncrementByTwoDayFromMonday()
        {
            var startDate = new DateTime(2015, 01, 31, 8, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Monday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 2);
            Assert.AreEqual(startDate.AddDays(1).AddHours(8).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void WorkdayIncrementBy66FromMonday()
        {
            var startDate = new DateTime(2015, 1, 15, 9, 44, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Monday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 0.66);
            Assert.AreEqual(startDate.Add(new TimeSpan(5, 16, 0)).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void WorkdayIncrementByOneAndQuarterDayFromMonday()
        {
            var startDate = new DateTime(2015, 01, 01, 8, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Monday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 1.25);
            Assert.AreEqual(startDate.Add(new TimeSpan(1, 2, 0, 0)).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void WorkdayIncrementBy333FromFriday()
        {
            var startDate = new DateTime(2015, 01, 20, 8, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Friday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 3.33);
            Assert.AreEqual(startDate.AddDays(5).AddHours(2).AddMinutes(38).ToShortDateString(), incrementResult.Date.ToShortDateString());
        }

        [TestMethod]
        public void WorkdayIncrementByStartDateOnSaturday()
        {
            var startDate = new DateTime(2015, 01, 20, 10, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Saturday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 1.25);
            Assert.AreEqual(startDate.AddDays(3).AddHours(2).ToShortDateString(), incrementResult.Date.ToShortDateString());
        }

        [TestMethod]
        public void WorkdayIncrementByStartDateOnSunday()
        {
            var startDate = new DateTime(2015, 01, 20, 9, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Sunday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 3.88);
            Assert.AreEqual(startDate.AddDays(4).AddHours(7).AddMinutes(2).ToShortDateString(), incrementResult.Date.ToShortDateString());
        }

        [TestMethod]
        public void OneWorkdayRequestFromMondayThuesdayIsHoliday()
        {
            var startDate = new DateTime(2015, 01, 20, 9, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Monday);
            workdayCalendar.SetHoliday(startDate.AddDays(1), false);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 1);
            Assert.AreEqual(startDate.Date.Add(new TimeSpan(2, 9, 0, 0)).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void OneWorkdayRequestFromMondayThuesdayIsRecurringHoliday()
        {
            var startDate = new DateTime(2015, 01, 12, 10, 0, 0);
            workdayCalendar.SetHoliday(new DateTime(2016, 01, 13), true);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, 1);
            Assert.AreEqual(startDate.AddDays(2).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void OneWorkingDayReverseRequestFromAWendsday()
        {
            var startDate = new DateTime(2015, 01, 12, 8, 0, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Wednesday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, -1);
            Assert.AreEqual(startDate.AddDays(-1).AsNumber(), incrementResult.AsNumber());
        }

        [TestMethod]
        public void MoveBackWithTimes()
        {
            var startDate = new DateTime(2015, 01, 12, 10, 55, 0);
            startDate = startDate.StartOfWeek(DayOfWeek.Wednesday);
            var incrementResult = workdayCalendar.GetWorkdayIncrement(startDate, -0.63258);
            Assert.AreEqual(startDate.AddDays(-1).Date.Add(new TimeSpan(13, 52, 0)).AsNumber(), incrementResult.AsNumber());
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff);
        }

        public static long AsNumber(this DateTime dt)
        {
            return long.Parse(dt.ToString("yyyyMMddHHmmss"));
        }
    }
}
