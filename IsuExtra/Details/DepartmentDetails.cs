using System;

namespace IsuExtra.Details
{
    public class DepartmentDetails
    {
        private readonly char _shortName;

        public DepartmentDetails(string name, char shortName)
        {
            Name = name ?? throw new ArgumentException("Null argument");
            ShortName = shortName;
        }

        public string Name { get; }

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
    }
}