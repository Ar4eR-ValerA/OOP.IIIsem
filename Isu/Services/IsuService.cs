using System.Collections.Generic;
using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new List<Group>();
        private readonly List<Student> _students = new List<Student>();

        public Group AddGroup(GroupName name)
        {
            var group = new Group(name, 30);
            _groups.Add(group);

            return group;
        }

        public Student AddStudent(Group @group, string name)
        {
            if (group.StudentsLimit <= group.Students.Count)
            {
                throw new IsuException("Group is full");
            }

            var student = new Student(_students.Count, name);
            group.Students.Add(student);
            _students.Add(student);
            student.Group = group;

            return student;
        }

        public Student GetStudent(int id)
        {
            try
            {
                return _students[id];
            }
            catch
            {
                return null;
            }
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in _students)
            {
                if (student.FullName == name)
                {
                    return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.Name.Equals(groupName))
                {
                    return group.Students;
                }
            }

            return new List<Student>();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = new List<Student>();

            foreach (Group group in _groups)
            {
                if (group.Name.Course.Equals(courseNumber))
                {
                    students.AddRange(group.Students);
                }
            }

            return students;
        }

        public Group FindGroup(GroupName groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.Name.Equals(groupName))
                {
                    return group;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = new List<Group>();

            foreach (Group group in _groups)
            {
                if (group.Name.Course.Equals(courseNumber))
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (newGroup.StudentsLimit <= newGroup.Students.Count)
            {
                throw new IsuException("Group is full");
            }

            foreach (Group group in _groups)
            {
                if (group.Name.Equals(student.Group.Name))
                {
                    group.Students.Remove(student);
                }
            }

            newGroup.Students.Add(student);
            student.Group = newGroup;
        }
    }
}