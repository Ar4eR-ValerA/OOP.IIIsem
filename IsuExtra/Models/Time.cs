using System;
using IsuExtra.Enums;

namespace IsuExtra.Models
{
    public class Time
    {
        private int _hour;
        private int _minute;

        public Time(WeekDay weekDay, int hour, int minute)
        {
            WeekDay = weekDay;
            Hour = hour;
            Minute = minute;
        }

        public WeekDay WeekDay { get; private set; }

        public int Hour
        {
            get => _hour;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Negative hour: {value}");
                }

                WeekDay += value / 24;
                _hour = value % 24;
            }
        }

        public int Minute
        {
            get => _minute;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Negative minute: {value}");
                }

                _hour += value / 60;
                _minute = value % 60;
            }
        }

        public static bool operator ==(Time left, Time right)
        {
            if (right is not null && left is not null)
            {
                return left.WeekDay == right.WeekDay &&
                       left._hour < right._hour &&
                       left._minute < right._minute;
            }

            return false;
        }

        public static bool operator !=(Time left, Time right)
        {
            return !(left == right);
        }

        public static bool operator <(Time left, Time right)
        {
            if (left.WeekDay != right.WeekDay)
            {
                return left.WeekDay < right.WeekDay;
            }

            if (left._hour != right._hour)
            {
                return left._hour < right._hour;
            }

            if (left._minute != right._minute)
            {
                return left._minute < right._minute;
            }

            return false;
        }

        public static bool operator >(Time left, Time right)
        {
            if (left.WeekDay != right.WeekDay)
            {
                return left.WeekDay > right.WeekDay;
            }

            if (left._hour != right._hour)
            {
                return left._hour > right._hour;
            }

            if (left._minute != right._minute)
            {
                return left._minute > right._minute;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_hour, _minute, (int)WeekDay);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && Equals((Time)obj);
        }

        private bool Equals(Time other)
        {
            return _hour == other._hour && _minute == other._minute && WeekDay == other.WeekDay;
        }
    }
}