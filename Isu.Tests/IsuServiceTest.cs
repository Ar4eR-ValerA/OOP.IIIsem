using System.Collections.Generic;
using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group group = _isuService.AddGroup(new GroupName("M3103"));
            Student student = _isuService.AddStudent(group, "Шевченко Валерий Владимирович");

            Assert.IsTrue(student.Group.Name.Equals(group.Name) && group.Students.Contains(student));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Group group = _isuService.AddGroup(new GroupName("M3113"));
            for (int i = 0; i < 30; i++)
            {
                _isuService.AddStudent(group, i.ToString());
            }
            
            Assert.Catch<IsuException>(() =>
            {

                _isuService.AddStudent(group, "31");
            });
        }
        
        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(new GroupName("M3812"));
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group1 = _isuService.AddGroup(new GroupName("M3101"));
            Group group2 = _isuService.AddGroup(new GroupName("M3102"));
            Student student = _isuService.AddStudent(group1, "Шевченко Валерий Владимирович");
            
            _isuService.ChangeStudentGroup(student, group2);
            Assert.AreEqual(group2.Name, student.Group.Name);
            Assert.IsTrue(group2.Students.Contains(student));
        }

        [Test]
        public void ManuallyChangeStudentGroup_ThrowException()
        {
            Group group1 = _isuService.AddGroup(new GroupName("M3101"));
            Group group2 = _isuService.AddGroup(new GroupName("M3102"));
            Student student = _isuService.AddStudent(group1, "Тестовое имя");
            
            Assert.Catch<IsuException>(() =>
            {
                student.Group = group2;
            });
        }

        [Test]
        public void GetStudent_GetCorrectStudent()
        {
            Group group = _isuService.AddGroup(new GroupName("M3112"));
            Student student = _isuService.AddStudent(group, "1");

            Assert.AreEqual(student, _isuService.GetStudent(student.Id));
            Assert.AreEqual(student, _isuService.GetStudent(student.Id));
        }

        [Test]
        public void FindWrongStudent_GetNull()
        {
            _isuService.AddGroup(new GroupName("M3112"));
            var student = new Student(0, "1", new Group("M3112", 30));

            Assert.IsNull(_isuService.FindStudent(student.FullName));
        }
        
        [Test]
        public void GetWrongStudent_ThrowException()
        {
            _isuService.AddGroup(new GroupName("M3112"));
            var student = new Student(0, "1", new Group("M3112", 30));

            Assert.Catch<IsuException>(() =>
            {
                _isuService.GetStudent(student.Id);
            });
        }
        
        [Test]
        public void FindStudents_GetCorrectStudentsList()
        {
            Group group = _isuService.AddGroup(new GroupName("M3112"));
            Student student1 = _isuService.AddStudent(group, "1");
            Student student2 = _isuService.AddStudent(group, "2");
            var students = new List<Student>
            {
                student1,
                student2
            };

            Assert.AreEqual(students, _isuService.FindStudents(group.Name));
            Assert.AreEqual(students, _isuService.FindStudents(new CourseNumber(1)));
        }

        [Test]
        public void GetGroup_GetCorrectGroup()
        {
            Group group = _isuService.AddGroup(new GroupName("M3112"));
            _isuService.AddStudent(group, "1");
            _isuService.AddStudent(group, "2");
            
            Assert.AreEqual(group, _isuService.FindGroup(group.Name));
        }
        
        public void GetGroups_GetCorrectGroupsList()
        {
            Group group1 = _isuService.AddGroup(new GroupName("M3112"));
            Group group2 = _isuService.AddGroup(new GroupName("M3113"));

            var groups = new List<Group>
            {
                group1,
                group2
            };
            
            Assert.AreEqual(groups, _isuService.FindGroups(new CourseNumber(1)));
        }
    }
}