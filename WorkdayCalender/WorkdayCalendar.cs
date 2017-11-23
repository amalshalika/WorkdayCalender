using System;
using System.Collections.Generic;

namespace Ti.Poc.Calendar
{
    public class WorkdayCalendar
    {
        TimeSpan startTimeOfDay;
        TimeSpan endTimeOfDay;
        List<Holiday> holidayList;
        bool isBackwardMove = false;
        public void SetWorkDayStartAndStop(TimeSpan startTime, TimeSpan endTime)
        {
            startTimeOfDay = startTime;
            endTimeOfDay = endTime;
            holidayList = new List<Holiday>();
        }
        public DateTime GetWorkdayIncrement(DateTime startDate, double distance)
        {
            isBackwardMove = distance < 0;

            if (IsInWeekendOrHoliday(startDate))
            {
                startDate = AdjustDateForHolidayAndWeekend(InitilizeStartTimeOfWorkingDay(startDate));
            }
            var fractionalDistance = distance - Math.Truncate(distance);
            var planingWorkingDay = TuneWorkingDayByFractional(startDate, fractionalDistance);
            planingWorkingDay = TuneWorkingDayByDays(planingWorkingDay, (int)Math.Truncate(distance), false);

            return planingWorkingDay;
        }
        private DateTime TuneWorkingDayByFractional(DateTime startDate, double fractionalIncrement)
        {
            var planingStartTime = CountFractionalPartAsTime(Math.Abs(fractionalIncrement));

            if(isBackwardMove)
            {
                return TuneWorkingDayStartTimeForBackwardMove(startDate, planingStartTime);
            }
            else
            {
                return TuneWorkingDayStartTimeForForwardMove(startDate, planingStartTime);
            }

        }
        public DateTime TuneWorkingDayStartTimeForForwardMove(DateTime startDate, TimeSpan ajustment)
        {
            if (startDate.TimeOfDay <= startTimeOfDay)
            {
                startDate = InitilizeStartTimeOfWorkingDay(startDate);
                return startDate + ajustment;
            }
            else if (startDate.TimeOfDay >= endTimeOfDay)
            {
                startDate = InitilizeStartTimeOfWorkingDay(startDate);
                return TuneWorkingDayByDays(startDate, 1, true) + ajustment;
            }

            var remainWorkingTimeOfDay = CountRemainWorkingTimeOfDay(startDate);
            if (remainWorkingTimeOfDay >= ajustment)
            {
                return startDate + ajustment;
            }

            var timeCarryToNextDay = ajustment - remainWorkingTimeOfDay;

            return TuneWorkingDayByDays(InitilizeStartTimeOfWorkingDay(startDate), 1, true) + timeCarryToNextDay;
        }

        public DateTime TuneWorkingDayStartTimeForBackwardMove(DateTime startDate, TimeSpan ajustment)
        {
            if (startDate.TimeOfDay <= startTimeOfDay)
            {
                startDate = InitilizeStartTimeOfWorkingDay(startDate);
                return TuneWorkingDayByDays(startDate, -1, true) - ajustment;
            }
            else if (startDate.TimeOfDay >= endTimeOfDay)
            {
                startDate = InitilizeStartTimeOfWorkingDay(startDate);
                return startDate - ajustment;
            }

            var remainWorkingTimeOfDay = CountRemainWorkingTimeOfDay(startDate);
            if (remainWorkingTimeOfDay >= ajustment)
            {
                return startDate - ajustment;
            }

            var timeCarryToNextDay = ajustment - remainWorkingTimeOfDay;
            return TuneWorkingDayByDays(InitilizeStartTimeOfWorkingDay(startDate), -1, true) - timeCarryToNextDay;
        }
        private TimeSpan CountRemainWorkingTimeOfDay(DateTime startDate)
        {
            return isBackwardMove ? startDate.TimeOfDay - startTimeOfDay : endTimeOfDay - startDate.TimeOfDay;
        }
        private DateTime InitilizeStartTimeOfWorkingDay(DateTime startDate)
        {
            return isBackwardMove ? startDate.Date.Add(endTimeOfDay) : startDate.Date.Add(startTimeOfDay);
        }
        private TimeSpan CountFractionalPartAsTime(double fractionalIncrement)
        {
            double hoursDiff = (endTimeOfDay - startTimeOfDay).TotalHours;
            var workingHours = fractionalIncrement * hoursDiff;
            var workingMinites = (int)Math.Truncate((workingHours - Math.Truncate(workingHours)) * 60);
            return new TimeSpan(Convert.ToInt16(Math.Truncate(workingHours)), workingMinites, 0);
        }
        private DateTime TuneWorkingDayByDays(DateTime startDate, int days, bool incrementByDay)
        {
            var dayToMove = isBackwardMove ? -1 : 1;
            for (int i = 0; i < Math.Abs(days); i++)
            {
                startDate = incrementByDay ? AdjustDateForHolidayAndWeekend(startDate.AddDays(dayToMove)) : TuneWorkingDayByFractional(startDate, dayToMove);
            }
            return startDate;
        }
        private DateTime AdjustDateForHolidayAndWeekend(DateTime startDate)
        {
            var dayToMove = isBackwardMove ? -1 : 1;

            if (IsInWeekendOrHoliday(startDate))
            {
                var dayToChange = startDate.AddDays(dayToMove);
                return AdjustDateForHolidayAndWeekend(dayToChange);
            }
            return startDate;
        }
        private bool IsInWeekendOrHoliday(DateTime startDate)
        {
            if ((startDate.DayOfWeek == DayOfWeek.Saturday) || (startDate.DayOfWeek == DayOfWeek.Sunday) || IsItHoliday(startDate.Date))
            {
                return true;
            }
            return false;
        }
        public void SetHoliday(DateTime dateTime, bool isRecur)
        {
            holidayList.Add(new Holiday() { Date = dateTime.Date, IsRecur = isRecur });
        }
        private bool IsItHoliday(DateTime dateTime)
        {
            foreach (var holiday in holidayList)
            {
                if (holiday.IsRecur && holiday.Date.Month == dateTime.Month && holiday.Date.Day == dateTime.Day)
                {
                    return true;
                }
                if (holiday.Date.CompareTo(dateTime) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class Holiday
    {
        public DateTime Date { get; set; }
        public bool IsRecur { get; set; }
    }
}
