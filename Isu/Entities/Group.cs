using System.Collections.Generic;
using Isu.Models;

namespace Isu.Entities
{
    public class Group
    {
        public Group(GroupName name, List<Student> students, int studentsLimit)
        {
            Name = name;
            Students = students;
            StudentsLimit = studentsLimit;
        }

        public Group(string name, List<Student> students, int studentsLimit)
        {
            Name = new GroupName(name);
            Students = students;
            StudentsLimit = studentsLimit;
        }

        public Group(GroupName name, int studentsLimit)
        {
            Name = name;
            Students = new List<Student>();
            StudentsLimit = studentsLimit;
        }

        public Group(string name, int studentsLimit)
        {
            Name = new GroupName(name);
            Students = new List<Student>();
            StudentsLimit = studentsLimit;
        }

        public GroupName Name { get; }
        public List<Student> Students { get; }
        public int StudentsLimit { get; }
    }
}