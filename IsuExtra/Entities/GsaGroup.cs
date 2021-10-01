using System;
using System.Collections.Generic;
using IsuExtra.Models;

namespace IsuExtra.Entities
{
    public class GsaGroup
    {
        private static int _idCounter;
        private readonly List<GsaStudent> _students;
        private int _studentsLimit;

        public GsaGroup(string name, int studentsLimit = 25)
        {
            Name = name ?? throw new ArgumentException("Null argument");
            StudentsLimit = studentsLimit;
            Schedule = new Schedule();
            _students = new List<GsaStudent>();
            Id = _idCounter++;
        }

        public int Id { get; }
        public Schedule Schedule { get; }
        public Gsa Gsa { get; internal set; }
        public IReadOnlyList<GsaStudent> Students => _students;

        public string Name { get; }

        public int StudentsLimit
        {
            get => _studentsLimit;
            private init
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Students' limits must be positive");
                }

                _studentsLimit = value;
            }
        }

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