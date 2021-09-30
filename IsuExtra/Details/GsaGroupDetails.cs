using System;

namespace IsuExtra.Details
{
    public class GsaGroupDetails
    {
        private int _studentsLimit;

        public GsaGroupDetails(string name)
        {
            Name = name ?? throw new ArgumentException("Null argument");
            StudentsLimit = 25;
        }

        public GsaGroupDetails(string name, int studentsLimit)
        {
            Name = name ?? throw new ArgumentException("Null argument");
            StudentsLimit = studentsLimit;
        }

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
    }
}