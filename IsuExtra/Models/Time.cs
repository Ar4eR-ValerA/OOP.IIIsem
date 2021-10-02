using System;

namespace IsuExtra.Models
{
    public class Time
    {
        public Time(DayOfWeek weekDay, TimeSpan timeSpan)
        {
            WeekDay = weekDay;
            TimeSpan = timeSpan;
        }

        public DayOfWeek WeekDay { get; }
        public TimeSpan TimeSpan { get; }
    }
}