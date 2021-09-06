using Isu.Tools;

namespace Isu.Entities
{
    public class Student
    {
        private Group _group;
        public Student(int id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public Student(int id, string fullName, Group group)
        {
            Id = id;
            FullName = fullName;
            _group = group;
        }

        public int Id { get; }
        public string FullName { get; }
        public Group Group
        {
            get => _group;
            set
            {
                if (!value.Students.Contains(this))
                {
                    throw new IsuException("Invalid group number");
                }

                _group = value;
            }
        }
    }
}