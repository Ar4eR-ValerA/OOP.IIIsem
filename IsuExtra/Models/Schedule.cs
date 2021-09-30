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
                if (lesson.StartTime < currentLesson.StartTime && lesson.EndTime > currentLesson.StartTime)
                {
                    return false;
                }

                if (lesson.StartTime > currentLesson.StartTime && lesson.StartTime < currentLesson.EndTime)
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