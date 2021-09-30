using System;
using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Details
{
    public class GsaStudentDetails
    {
        public GsaStudentDetails(Student student, Department department)
        {
            Student = student ?? throw new ArgumentException("Null argument");
            Department = department ?? throw new ArgumentException("Null argument");
        }

        public Student Student { get; }
        public Department Department { get; }
    }
}