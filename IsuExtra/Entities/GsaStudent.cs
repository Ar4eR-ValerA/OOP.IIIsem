using System;
using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Models;

namespace IsuExtra.Entities
{
    public class GsaStudent
    {
        private readonly List<GsaGroup> _gsaGroups;

        public GsaStudent(Student student, Department department)
        {
            Student = student ?? throw new ArgumentException("Null argument");
            Department = department ?? throw new ArgumentException("Null argument");
            Id = student.Id;
            _gsaGroups = new List<GsaGroup>();
            Schedule = new Schedule();
        }

        public Student Student { get; }
        public Department Department { get; }
        public int GsaLimit => 2;
        public int Id { get; }
        public IReadOnlyList<GsaGroup> GsaGroups => _gsaGroups;
        public Schedule Schedule { get; }

        internal void AddGsaGroup(GsaGroup gsaGroup)
        {
            if (gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            if (_gsaGroups.Count >= GsaLimit)
            {
                throw new ArgumentException("Gsas' list is full");
            }

            if (gsaGroup.Gsa.Department == Department)
            {
                throw new ArgumentException("Gsa's and student's department must be different");
            }

            Schedule.AddLessons(gsaGroup.Schedule.Lessons);
            _gsaGroups.Add(gsaGroup);
        }

        internal void RemoveGsaGroup(GsaGroup gsaGroup)
        {
            if (gsaGroup is null)
            {
                throw new ArgumentException("Null argument");
            }

            _gsaGroups.Remove(gsaGroup);
        }
    }
}