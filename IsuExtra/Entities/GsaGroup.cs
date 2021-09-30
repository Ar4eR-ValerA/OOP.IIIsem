using System;
using System.Collections.Generic;
using IsuExtra.Details;
using IsuExtra.Models;

namespace IsuExtra.Entities
{
    public class GsaGroup
    {
        private static int _idCounter;
        private readonly List<GsaStudent> _students;

        public GsaGroup(GsaGroupDetails gsaGroupDetails)
        {
            GsaGroupDetails = gsaGroupDetails ?? throw new ArgumentException("Null argument");
            Schedule = new Schedule();
            _students = new List<GsaStudent>();
            Id = _idCounter++;
        }

        public GsaGroupDetails GsaGroupDetails { get; }
        public int Id { get; }
        public Schedule Schedule { get; }
        public Gsa Gsa { get; internal set; }
        public IReadOnlyList<GsaStudent> Students => _students;

        internal void AddStudent(GsaStudent student)
        {
            if (student is null)
            {
                throw new ArgumentException("Null argument");
            }

            _students.Add(student);
        }

        internal void RemoveStudent(GsaStudent student)
        {
            if (student is null)
            {
                throw new ArgumentException("Null argument");
            }

            _students.Remove(student);
        }
    }
}