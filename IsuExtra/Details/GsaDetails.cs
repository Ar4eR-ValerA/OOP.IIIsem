using System;

namespace IsuExtra.Details
{
    public class GsaDetails
    {
        public GsaDetails(string name)
        {
            Name = name ?? throw new ArgumentException("Null argument");
        }

        public string Name { get; }
    }
}