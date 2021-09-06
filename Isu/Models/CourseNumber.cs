#nullable enable
using Isu.Tools;

namespace Isu.Models
{
    public class CourseNumber
    {
        private readonly int _course;

        public CourseNumber(int course)
        {
            if (course is < 0 or > 4)
            {
                throw new IsuException($"Invalid course: {course}");
            }

            _course = course;
        }

        public CourseNumber(char course)
        {
            int intCourse = course - '0';
            if (intCourse is < 0 or > 4)
            {
                throw new IsuException($"Invalid course: {course}");
            }

            _course = intCourse;
        }

        public override string ToString()
        {
            return _course.ToString();
        }

        public bool Equals(CourseNumber value)
        {
            return _course == value._course;
        }

        public override bool Equals(object? obj)
        {
            return obj is CourseNumber number && Equals(number);
        }

        public override int GetHashCode()
        {
            return _course;
        }
    }
}