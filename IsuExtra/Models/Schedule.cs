using System;
using System.Collections.Generic;
using System.Linq;

namespace IsuExtra.Models
{
    public class Schedule
    {
        private readonly List<Lesson> _lessons;

        public Schedule()
        {
            _lessons = new List<Lesson>();
        }

        public Schedule(List<Lesson> lessons)
        {
            _lessons = lessons;
        }

        public IReadOnlyList<Lesson> Lessons => _lessons;

        public bool IsTimeFree(Lesson lesson)
        {
            foreach (Lesson currentLesson in _lessons)
            {
                if (lesson.StartTime.TimeSpan < currentLesson.StartTime.TimeSpan &&
                    lesson.EndTime.TimeSpan > currentLesson.StartTime.TimeSpan)
                {
                    return false;
                }

                if (lesson.StartTime.TimeSpan > currentLesson.StartTime.TimeSpan &&
                    lesson.StartTime.TimeSpan < currentLesson.EndTime.TimeSpan)
                {
                    return false;
                }
            }

            return true;
        }

        internal void AddLesson(Lesson lesson)
        {
            AddLessons(new[] { lesson });
        }

        internal void AddLessons(IReadOnlyList<Lesson> lessons)
        {
            if (lessons.Any(lesson => !IsTimeFree(lesson)))
            {
                throw new ArgumentException("Lessons are crossing");
            }

            _lessons.AddRange(lessons);
        }
    }
}