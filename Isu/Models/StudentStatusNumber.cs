#nullable enable
using Isu.Tools;

namespace Isu.Models
{
    public class StudentStatusNumber
    {
        private readonly int _studentStatus;

        public StudentStatusNumber(int studentStatus)
        {
            if (studentStatus != 3)
            {
                throw new IsuException("Invalid student status");
            }

            _studentStatus = studentStatus;
        }

        public StudentStatusNumber(char studentStatus)
        {
            int intStudentStatus = studentStatus - '0';
            if (intStudentStatus != 3)
            {
                throw new IsuException("Invalid student status");
            }

            _studentStatus = intStudentStatus;
        }

        public override string ToString()
        {
            return _studentStatus.ToString();
        }

        public bool Equals(StudentStatusNumber value)
        {
            return _studentStatus == value._studentStatus;
        }

        public override bool Equals(object? obj)
        {
            return obj is StudentStatusNumber number && Equals(number);
        }

        public override int GetHashCode()
        {
            return _studentStatus;
        }
    }
}