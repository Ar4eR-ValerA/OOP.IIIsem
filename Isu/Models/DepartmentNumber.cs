#nullable enable
using Isu.Tools;

namespace Isu.Models
{
    public class DepartmentNumber
    {
        private readonly char _department;

        public DepartmentNumber(char department)
        {
            if (department != 'M')
            {
                throw new IsuException($"Invalid department: {department}");
            }

            _department = department;
        }

        public override string ToString()
        {
            return _department.ToString();
        }

        public bool Equals(DepartmentNumber value)
        {
            return _department == value._department;
        }

        public override bool Equals(object? obj)
        {
            return obj is DepartmentNumber number && Equals(number);
        }

        public override int GetHashCode()
        {
            return _department;
        }
    }
}