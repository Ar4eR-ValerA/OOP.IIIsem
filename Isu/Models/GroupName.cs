#nullable enable
using System;

namespace Isu.Models
{
    public class GroupName
    {
        public GroupName(
            DepartmentNumber department,
            StudentStatusNumber statusNumber,
            CourseNumber course,
            GroupNumber group)
        {
            Department = department;
            StatusNumber = statusNumber;
            Course = course;
            Group = group;
        }

        public GroupName(string groupName)
        {
            Department = new DepartmentNumber(groupName[0]);
            StatusNumber = new StudentStatusNumber(groupName[1]);
            Course = new CourseNumber(groupName[2]);
            Group = new GroupNumber(groupName[3..]);
        }

        public DepartmentNumber Department { get; }
        public StudentStatusNumber StatusNumber { get; }
        public CourseNumber Course { get; }
        public GroupNumber Group { get; }

        public override string ToString()
        {
            return $"{Department}{StatusNumber}{Course}{Group}";
        }

        public bool Equals(GroupName value)
        {
            return Department.Equals(value.Department) &&
                   StatusNumber.Equals(value.StatusNumber) &&
                   Course.Equals(value.Course) &&
                   Group.Equals(value.Group);
        }

        public override bool Equals(object? obj)
        {
            return obj is GroupName name && Equals(name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Department, StatusNumber, Course, Group);
        }
    }
}