using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Models;
using Isu.Services;
using IsuExtra.Details;
using NUnit.Framework;
using IsuExtra.Entities;
using IsuExtra.Enums;
using IsuExtra.Models;

namespace IsuExtra.Tests
{
    public class Tests
    {
        private IsuService _isuService;
        private GsaService _gsaService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
            _gsaService = new GsaService();
        }

        [Test]
        public void AddIncorrectDepartment_ThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new DepartmentDetails(null, 'M');
            });
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new DepartmentDetails("Test", 's');
            });
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new DepartmentDetails("Test", '2');
            });
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new DepartmentDetails("Test", ' ');
            });
        }

        [Test]
        public void AddGsa_GsaInAdded()
        {
            var departmentDetails = new DepartmentDetails("Cyberpunk", 'K');
            var gsaDetails = new GsaDetails("KIB");

            var department = new Department(departmentDetails);
            var gsa = new Gsa(gsaDetails);

            _gsaService.RegisterDepartment(department);
            _gsaService.RegisterGsa(gsa, department);

            Assert.Contains(gsa, department.Gsas.ToList());
        }

        [Test]
        public void AddIncorrectGsa_ThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Gsa(null);
            });
        }

        [Test]
        [TestCase(13, 30, 15, 10)]
        public void EnrollStudent_StudentEnrolled(int hour1, int minute1, int hour2, int minute2)
        {
            Group group = _isuService.AddGroup(new GroupName("M3201"));
            Student student = _isuService.AddStudent(group, "Michael");

            var departmentDetails1 = new DepartmentDetails("Cyberpunk", 'K');
            var departmentDetails2 = new DepartmentDetails("ITIP", 'M');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails1 = new GsaGroupDetails("KIB 1.1");
            var gsaGroupDetails2 = new GsaGroupDetails("BIK 1.4");

            var department1 = new Department(departmentDetails1);
            var department2 = new Department(departmentDetails2);
            var gsa = new Gsa(gsaDetails);
            var gsaGroup1 = new GsaGroup(gsaGroupDetails1);
            var gsaGroup2 = new GsaGroup(gsaGroupDetails2);

            var gsaStudentDetails = new GsaStudentDetails(student, department2);
            var gsaStudent = new GsaStudent(gsaStudentDetails);

            _gsaService.RegisterDepartment(department1);
            _gsaService.RegisterDepartment(department2);
            _gsaService.RegisterGsa(gsa, department1);
            _gsaService.AddGsaGroup(gsaGroup1, gsa);
            _gsaService.AddGsaGroup(gsaGroup2, gsa);

            var lesson1 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour1, minute1),
                new Mentor("Fredi"),
                "461a");
            var lesson2 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour2, minute2),
                new Mentor("Fredi"),
                "461a");

            _gsaService.AddLesson(lesson1, gsaGroup1);
            _gsaService.AddLesson(lesson2, gsaGroup2);

            _gsaService.EnrollStudent(gsaStudent, gsaGroup1);
            _gsaService.EnrollStudent(gsaStudent, gsaGroup2);

            Assert.Contains(gsaStudent, _gsaService.FindGsaStudents(gsaGroup1).ToList());
            Assert.Contains(gsaStudent, _gsaService.FindGsaStudents(gsaGroup2).ToList());
        }

        [Test]
        [TestCase(13, 30, 13, 40)]
        public void EnrollStudentWithCrossingLessons_ThrowException(int hour1, int minute1, int hour2, int minute2)
        {
            Group group = _isuService.AddGroup(new GroupName("M3201"));
            Student student = _isuService.AddStudent(group, "Michael");

            var departmentDetails1 = new DepartmentDetails("Cyberpunk", 'K');
            var departmentDetails2 = new DepartmentDetails("ITIP", 'M');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails1 = new GsaGroupDetails("KIB 1.1");
            var gsaGroupDetails2 = new GsaGroupDetails("BIK 1.4");

            var department1 = new Department(departmentDetails1);
            var department2 = new Department(departmentDetails2);
            var gsa = new Gsa(gsaDetails);
            var gsaGroup1 = new GsaGroup(gsaGroupDetails1);
            var gsaGroup2 = new GsaGroup(gsaGroupDetails2);

            _gsaService.RegisterDepartment(department1);
            _gsaService.RegisterDepartment(department2);
            _gsaService.RegisterGsa(gsa, department1);
            _gsaService.AddGsaGroup(gsaGroup1, gsa);
            _gsaService.AddGsaGroup(gsaGroup2, gsa);

            var lesson1 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour1, minute1),
                new Mentor("Fredi"),
                "461a");
            var lesson2 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour2, minute2),
                new Mentor("Vlad"),
                "228l");

            _gsaService.AddLesson(lesson1, gsaGroup1);
            _gsaService.AddLesson(lesson2, gsaGroup2);

            var gsaStudentDetails = new GsaStudentDetails(student, department2);
            var gsaStudent = new GsaStudent(gsaStudentDetails);

            _gsaService.EnrollStudent(gsaStudent, gsaGroup1);
            Assert.Catch<ArgumentException>(() =>
            {
                _gsaService.EnrollStudent(gsaStudent, gsaGroup2);
            });
        }

        [Test]
        [TestCase(13, 30)]
        public void EnrollStudentWithSameDepartment_ThrowException(int hour, int minute)
        {
            Group group = _isuService.AddGroup(new GroupName("M3201"));
            Student student = _isuService.AddStudent(group, "Michael");

            var departmentDetails = new DepartmentDetails("Cyberpunk", 'M');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails = new GsaGroupDetails("KIB 1.1");

            var gsa = new Gsa(gsaDetails);
            var department = new Department(departmentDetails);
            var gsaGroup = new GsaGroup(gsaGroupDetails);

            _gsaService.RegisterGsa(gsa, department);
            _gsaService.AddGsaGroup(gsaGroup, gsa);

            var lesson = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour, minute),
                new Mentor("Fredi"),
                "461a");

            _gsaService.AddLesson(lesson, gsaGroup);

            var gsaStudentDetails = new GsaStudentDetails(student, department);
            var gsaStudent = new GsaStudent(gsaStudentDetails);

            Assert.Catch<ArgumentException>(() =>
            {
                _gsaService.EnrollStudent(gsaStudent, gsaGroup);
            });
        }

        [Test]
        [TestCase(13, 30, 15, 10)]
        public void GetGsaGroups_GotGsaGroups(int hour1, int minute1, int hour2, int minute2)
        {
            var departmentDetails = new DepartmentDetails("Cyberpunk", 'K');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails1 = new GsaGroupDetails("KIB 1.1");
            var gsaGroupDetails2 = new GsaGroupDetails("KIB 1.2");

            var gsa = new Gsa(gsaDetails);
            var department = new Department(departmentDetails);
            var gsaGroup1 = new GsaGroup(gsaGroupDetails1);
            var gsaGroup2 = new GsaGroup(gsaGroupDetails2);

            _gsaService.RegisterGsa(gsa, department);
            _gsaService.AddGsaGroup(gsaGroup1, gsa);
            _gsaService.AddGsaGroup(gsaGroup2, gsa);

            var lesson1 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour1, minute1),
                new Mentor("Fredi"),
                "461a");
            var lesson2 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour2, minute2),
                new Mentor("Vlad"),
                "228l");

            _gsaService.AddLesson(lesson1, gsaGroup1);
            _gsaService.AddLesson(lesson2, gsaGroup2);


            IReadOnlyList<GsaGroup> gsaGroups = _gsaService.FindGsaGroups(gsa);

            Assert.Contains(gsaGroup1, gsaGroups.ToList());
            Assert.Contains(gsaGroup2, gsaGroups.ToList());
        }

        [Test]
        [TestCase(13, 30, 14, 10)]
        public void AddGsaGroupWithCrossingLessons_ThrowException(int hour1, int minute1, int hour2, int minute2)
        {
            var departmentDetails = new DepartmentDetails("Cyberpunk", 'K');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails = new GsaGroupDetails("KIB 1.1");

            var gsa = new Gsa(gsaDetails);
            var department = new Department(departmentDetails);
            var gsaGroup = new GsaGroup(gsaGroupDetails);

            _gsaService.RegisterGsa(gsa, department);
            _gsaService.AddGsaGroup(gsaGroup, gsa);

            var lesson1 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour1, minute1),
                new Mentor("Fredi"),
                "461a");
            var lesson2 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour2, minute2),
                new Mentor("Vlad"),
                "228l");

            _gsaService.AddLesson(lesson1, gsaGroup);
            Assert.Catch<ArgumentException>(() =>
            {
                _gsaService.AddLesson(lesson2, gsaGroup);
            });
        }

        [Test]
        [TestCase(13, 30)]
        public void GetGsaStudents_GotGsaStudents(int hour, int minute)
        {
            var departmentDetails1 = new DepartmentDetails("Cyberpunk", 'K');
            var departmentDetails2 = new DepartmentDetails("ITIP", 'M');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails = new GsaGroupDetails("KIB 1.1");

            var gsa = new Gsa(gsaDetails);
            var department1 = new Department(departmentDetails1);
            var department2 = new Department(departmentDetails2);
            var gsaGroup = new GsaGroup(gsaGroupDetails);

            _gsaService.RegisterGsa(gsa, department1);
            _gsaService.AddGsaGroup(gsaGroup, gsa);

            var lesson1 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour, minute),
                new Mentor("Fredi"),
                "461a");

            _gsaService.AddLesson(lesson1, gsaGroup);

            Group group = _isuService.AddGroup(new GroupName("M3201"));
            Student student1 = _isuService.AddStudent(group, "Michael");
            Student student2 = _isuService.AddStudent(group, "Michael");

            var gsaStudent1Details = new GsaStudentDetails(student1, department2);
            var gsaStudent2Details = new GsaStudentDetails(student2, department2);
            var gsaStudent1 = new GsaStudent(gsaStudent1Details);
            var gsaStudent2 = new GsaStudent(gsaStudent2Details);

            _gsaService.EnrollStudent(gsaStudent1, gsaGroup);
            _gsaService.EnrollStudent(gsaStudent2, gsaGroup);

            Assert.Contains(gsaStudent1, _gsaService.FindGsaStudents(gsaGroup).ToList());
            Assert.Contains(gsaStudent2, _gsaService.FindGsaStudents(gsaGroup).ToList());
        }
        
        [Test]
        [TestCase(13, 30)]
        public void GetNotGsaStudents_GotNotGsaStudents(int hour, int minute)
        {
            var departmentDetails1 = new DepartmentDetails("Cyberpunk", 'K');
            var departmentDetails2 = new DepartmentDetails("ITIP", 'M');
            var gsaDetails = new GsaDetails("KIB");
            var gsaGroupDetails = new GsaGroupDetails("KIB 1.1");

            var gsa = new Gsa(gsaDetails);
            var department1 = new Department(departmentDetails1);
            var department2 = new Department(departmentDetails2);
            var gsaGroup = new GsaGroup(gsaGroupDetails);
            
            _gsaService.RegisterGsa(gsa, department1);
            _gsaService.AddGsaGroup(gsaGroup, gsa);

            var lesson1 = new Lesson(
                "OOP",
                new Time(WeekDay.Friday, hour, minute),
                new Mentor("Fredi"),
                "461a");

            _gsaService.AddLesson(lesson1, gsaGroup);

            Group group = _isuService.AddGroup(new GroupName("M3201"));
            Student student1 = _isuService.AddStudent(group, "Michael");
            Student student2 = _isuService.AddStudent(group, "Vlad");

            var gsaStudent1Details = new GsaStudentDetails(student1, department2);
            var gsaStudent2Details = new GsaStudentDetails(student2, department2);
            var gsaStudent1 = new GsaStudent(gsaStudent1Details);
            var gsaStudent2 = new GsaStudent(gsaStudent2Details);

            _gsaService.EnrollStudent(gsaStudent1, gsaGroup);

            Assert.Contains(student1, _gsaService.FindNotGsaStudents(group).ToList());
            Assert.AreEqual(1, _gsaService.FindNotGsaStudents(group).Count);
        }
    }
}