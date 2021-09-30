using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using IsuExtra.Entities;
using IsuExtra.Models;

namespace IsuExtra
{
    public class GsaService
    {
        private readonly Dictionary<int, Gsa> _gsas;
        private readonly Dictionary<int, Department> _departments;
        private readonly Dictionary<int, GsaStudent> _gsaStudents;
        private readonly Dictionary<int, GsaGroup> _gsaGroups;

        public GsaService()
        {
            _gsas = new Dictionary<int, Gsa>();
            _departments = new Dictionary<int, Department>();
            _gsaStudents = new Dictionary<int, GsaStudent>();
            _gsaGroups = new Dictionary<int, GsaGroup>();
        }

        public void RegisterDepartment(Department department)
        {
            if (department is null)
            {
                throw new ArgumentException("Null argument");
            }

            _departments.Add(department.Id, department);
        }

        public void RegisterGsa(Gsa gsa, Department department)
        {
            if (department is null || gsa is null)
            {
                throw new ArgumentException("Null argument");
            }

            department.AddGsa(gsa);
            gsa.Department = department;
            _gsas.Add(gsa.Id, gsa);
        }

        public void AddGsaGroup(GsaGroup gsaGroup, Gsa gsa)
        {
            if (gsa is null || gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            gsa.AddGsaGroup(gsaGroup);
            gsaGroup.Gsa = gsa;
            _gsaGroups.Add(gsaGroup.Id, gsaGroup);
        }

        public void EnrollStudent(GsaStudent gsaStudent, GsaGroup gsaGroup)
        {
            if (gsaStudent is null || gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            gsaStudent.AddGsaGroup(gsaGroup);
            gsaGroup.AddStudent(gsaStudent);
            if (!_gsaStudents.ContainsKey(gsaStudent.Id))
            {
                _gsaStudents.Add(gsaStudent.Id, gsaStudent);
            }
        }

        public void ExpelStudent(GsaStudent student, GsaGroup gsaGroup)
        {
            if (student is null || gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            gsaGroup.RemoveStudent(student);
            student.RemoveGsaGroup(gsaGroup);
            if (student.GsaGroups.Count == 0)
            {
                _gsaStudents.Remove(student.Id);
            }
        }

        public void AddLesson(Lesson lesson, GsaGroup gsaGroup)
        {
            if (lesson is null || gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            gsaGroup.Schedule.AddLesson(lesson);
            lesson.GsaGroup = gsaGroup;
        }

        public IReadOnlyList<GsaGroup> FindGsaGroups(Gsa gsa)
        {
            if (gsa is null)
            {
                throw new ArgumentException("Null argument");
            }

            return gsa.GsaGroups;
        }

        public IReadOnlyList<GsaStudent> FindGsaStudents(GsaGroup gsaGroup)
        {
            if (gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            return gsaGroup.Students;
        }

        public IReadOnlyList<Student> FindNotGsaStudents(Group group)
        {
            if (group is null)
            {
                throw new ArgumentException("Null argument");
            }

            var notGsaStudents = new List<Student>();

            foreach (Student student in group.Students)
            {
                notGsaStudents.AddRange(_gsaStudents.Values
                    .Where(gsaStudent => gsaStudent.StudentDetails.Student == student)
                    .Select(gsaStudent => student));
            }

            return notGsaStudents;
        }
    }
}