using System;
using System.Collections.Generic;

namespace IsuExtra.Entities
{
    public class Department
    {
        private static int _idCounter;
        private readonly List<Gsa> _gsas;
        private readonly char _shortName;

        public Department(string name, char shortName)
        {
            Name = name ?? throw new ArgumentException("Null argument");
            ShortName = shortName;
            _gsas = new List<Gsa>();
            Id = _idCounter++;
        }

        public char ShortName
        {
            get => _shortName;
            private init
            {
                if (!char.IsUpper(value))
                {
                    throw new ArgumentException("Short name must be upper case letter");
                }

                _shortName = value;
            }
        }

        public string Name { get; }
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