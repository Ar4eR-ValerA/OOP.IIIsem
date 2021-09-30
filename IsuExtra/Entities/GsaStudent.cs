using System;
using System.Collections.Generic;
using IsuExtra.Details;
using IsuExtra.Models;

namespace IsuExtra.Entities
{
    public class GsaStudent
    {
        private static int _idCounter;
        private readonly List<GsaGroup> _gsaGroups;

        public GsaStudent(GsaStudentDetails studentDetails)
        {
            StudentDetails = studentDetails ?? throw new ArgumentException("Null argument");
            Id = _idCounter++;
            _gsaGroups = new List<GsaGroup>();
            Schedule = new Schedule();
        }

        public int GsaLimit => 2;
        public int Id { get; }
        public IReadOnlyList<GsaGroup> GsaGroups => _gsaGroups;
        public GsaStudentDetails StudentDetails { get; }
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

            if (gsaGroup.Gsa.Department == StudentDetails.Department)
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