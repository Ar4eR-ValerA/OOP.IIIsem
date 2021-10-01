using System;
using IsuExtra.Enums;

namespace IsuExtra.Models
{
    public class Time
    {
        public Time(WeekDay weekDay, TimeSpan timeSpan)
        {
            WeekDay = weekDay;
            TimeSpan = timeSpan;
        }

        public WeekDay WeekDay { get; }
        public TimeSpan TimeSpan { get; }
    }
}