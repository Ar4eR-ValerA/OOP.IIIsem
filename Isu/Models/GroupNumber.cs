#nullable enable
using System;
using Isu.Tools;

namespace Isu.Models
{
    public class GroupNumber
    {
        private readonly string _group;

        public GroupNumber(int group)
        {
            if (group is < 0 or > 99)
            {
                throw new IsuException("Invalid group");
            }

            _group = group.ToString();
        }

        public GroupNumber(string group)
        {
            if (Convert.ToInt32(group) is < 0 or > 99)
            {
                throw new IsuException("Invalid group");
            }

            _group = group;
        }

        public override string ToString()
        {
            return _group;
        }

        public bool Equals(GroupNumber value)
        {
            return _group == value._group;
        }

        public override bool Equals(object? obj)
        {
            return obj is GroupNumber number && Equals(number);
        }

        public override int GetHashCode()
        {
            return _group.GetHashCode();
        }
    }
}