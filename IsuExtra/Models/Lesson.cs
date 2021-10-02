using System;
using IsuExtra.Entities;

namespace IsuExtra.Models
{
    public class Lesson
    {
        public Lesson(string name, Time startTime, int durationMinutes, Mentor mentor, string classroomNumber)
        {
            Name = name ?? throw new ArgumentException("Null argument");
            DurationMinutes = durationMinutes;

            StartTime = startTime ?? throw new ArgumentException("Null argument");
            EndTime = new Time(startTime.WeekDay, new TimeSpan(
                startTime.TimeSpan.Hours,
                startTime.TimeSpan.Minutes + DurationMinutes,
                startTime.TimeSpan.Seconds));

            Mentor = mentor ?? throw new ArgumentException("Null argument");
            ClassroomNumber = classroomNumber;
        }

        public Lesson(string name, Time startTime, Mentor mentor, string classroomNumber)
            : this(name, startTime, 90, mentor, classroomNumber)
        {
        }

        public string Name { get; }
        public Time StartTime { get; }
        public Time EndTime { get; }
        public int DurationMinutes { get; }
        public Mentor Mentor { get; set; }
        public GsaGroup GsaGroup { get; internal set; }
        public string ClassroomNumber { get; set; }
    }
}