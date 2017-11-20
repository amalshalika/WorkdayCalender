using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ti.Poc.Calendar.Test
{
    [TestClass]
    public class SecondTestClass
    {
        WorkdayCalendar workdayCalendar;
        [TestInitialize]
        public void Setup()
        {
            workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkDayStartAndStop(new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0));
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 17, 0, 0, 0), true);
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27, 0, 0, 0), false);
        }

        [TestMethod]
        public void WorkingDayDecrementBy5_5()
        {
            var startDate = new DateTime(2004, 5, 24, 18, 5, 0);
            var result = workdayCalendar.GetWorkdayIncrement(startDate, -5.5);
            Assert.AreEqual(new DateTime(2004, 5, 14, 12, 0, 0).AsNumber(), result.AsNumber());
        }


        [TestMethod]
        public void WorkingDayIncrementBy44_723656()
        {
            var startDate = new DateTime(2004, 5, 24, 19, 3, 0);
            var result = workdayCalendar.GetWorkdayIncrement(startDate, 44.723656);
            Assert.AreEqual(new DateTime(2004, 7, 27, 13, 47, 0).AsNumber(), result.AsNumber());
        }

        [TestMethod]
        public void WorkingDayDecrementBy6_7470217 ()
        {
            var startDate = new DateTime(2004, 5, 24, 18, 3, 0);
            var result = workdayCalendar.GetWorkdayIncrement(startDate, -6.7470217);
            Assert.AreEqual(new DateTime(2004, 5, 13, 10, 2, 0).AsNumber(), result.AsNumber());
        }

        [TestMethod]
        public void WorkingDayIncrementBy12_782709 ()
        {
            var startDate = new DateTime(2004, 5, 24, 8, 3, 0);
            var result = workdayCalendar.GetWorkdayIncrement(startDate, 12.782709);
            Assert.AreEqual(new DateTime(2004, 6, 10, 14, 18, 0).AsNumber(), result.AsNumber());
        }

        [TestMethod]
        public void WorkingDayIncrementBy8_276628 ()
        {
            var startDate = new DateTime(2004, 5, 24, 7, 3, 0);
            var result = workdayCalendar.GetWorkdayIncrement(startDate, 8.276628);
            Assert.AreEqual(new DateTime(2004, 6, 4, 10, 12, 0).AsNumber(), result.AsNumber());
        }
    }
}
