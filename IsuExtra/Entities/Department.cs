using System;
using System.Collections.Generic;
using IsuExtra.Details;

namespace IsuExtra.Entities
{
    public class Department
    {
        private static int _idCounter;
        private readonly List<Gsa> _gsas;

        public Department(DepartmentDetails departmentDetails)
        {
            DepartmentDetails = departmentDetails ?? throw new ArgumentException("Null argument");
            _gsas = new List<Gsa>();
            Id = _idCounter++;
        }

        public DepartmentDetails DepartmentDetails { get; }
        public int Id { get; }
        public IReadOnlyList<Gsa> Gsas => _gsas;

        internal void AddGsa(Gsa gsa)
        {
            if (gsa is null)
            {
                throw new ArgumentException("Null argument");
            }

            _gsas.Add(gsa);
        }
    }
}