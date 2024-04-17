using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RecourceCloud.Tests
{
    public class Tests
    {

        [Test]
        public void LogTask_Logs_A_Task_Successfully_And_Modifies_TaskCollection()
        {
            DepartmentCloud department = new DepartmentCloud();
            string result = department.LogTask(new string[] { "2", "Task 2", "Resource 2" });
            string expectedResult = "Task logged successfully.";

            Assert.AreEqual(1, department.Tasks.Count);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void LogTask_ThrowsArgumentExceptionWhen_Arguments_Are_Less_Than_Three_And_Does_NotAdd_The_Task()
        {
            DepartmentCloud department = new DepartmentCloud();

            Assert.IsEmpty(department.Tasks);
            Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { "6", "Task 101" }));
        }

        [Test]
        public void LogTask_ThrowsException_And_Returns_A_Message()
        {
            DepartmentCloud department = new DepartmentCloud();

            string expectedResult = "All arguments are required.";
            var exception = Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { "6", "Task 101" }));

            Assert.AreEqual(expectedResult, exception.Message);
        }

        [Test]
        public void LogTask_ThrowsArgumentException_If_Any_ArgumentIs_Null()
        {
            DepartmentCloud department = new DepartmentCloud();

            Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { "2", null, "Resource 2" }));
            Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { null, "Task 2", "Resource 2" }));
            Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { "2", "Task 2", null }));

        }

        [Test]
        public void LogTask_ThrowsException_And_Returns_A_Message_If_Any_Argument_Is_Null()
        {
            DepartmentCloud department = new DepartmentCloud();

            string expectedResult = "Arguments values cannot be null.";
            var exception = Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { "2", null, "Resource 2" }));
            var exception2 = Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { null, "Task 2", "Resource 2" }));
            var exception3 = Assert.Throws<ArgumentException>(() => department.LogTask(new string[] { "2", "Task 2", null }));

            Assert.AreEqual(expectedResult, exception.Message);
            Assert.AreEqual(expectedResult, exception2.Message);
            Assert.AreEqual(expectedResult, exception3.Message);
        }
        [Test]
        public void LogTask_TaskAlreadyLogged_ReturnsErrorMessage()
        {
            DepartmentCloud department = new DepartmentCloud();
            department.LogTask(new string[] { "5", "Task 5", "Resource 5" });

            string result = department.LogTask(new string[] { "5", "Task 5", "Resource 5" });
            string expectedResult = "Resource 5 is already logged.";

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CreateResource_CreatesResourceFromTask_And_Modifies_Collections()
        {
            DepartmentCloud department = new DepartmentCloud();
            department.LogTask(new string[] { "3", "Task 3", "Resource 3" });

            bool result = department.CreateResource();

            Assert.AreEqual(true, result);
            Assert.IsEmpty(department.Tasks);
            Assert.AreEqual(1, department.Resources.Count);
        }

        [Test]
        public void CreateResource_CreatesResourceWithCorrectResourceType()
        {
            DepartmentCloud department2 = new DepartmentCloud();
            string resourceName = "Resource1";
            string resourceType = "Task 1";

            department2.LogTask(new string[] { "1", "Task 1", resourceName });

            department2.CreateResource();

            Resource createdResource = department2.Resources.FirstOrDefault(r => r.Name == resourceName);

            Assert.IsNotNull(createdResource);
            Assert.AreEqual(resourceType, createdResource.ResourceType);
        }

        [Test]
        public void CreateResource_TaskIsNull_ReturnsFalseAndDoesNotModifyCollections()
        {
            DepartmentCloud department = new DepartmentCloud();

            bool result = department.CreateResource();

            Assert.IsFalse(result);
            Assert.IsEmpty(department.Resources);
            Assert.IsEmpty(department.Tasks);
        }

        [Test]
        public void CreateResource_ReturnsTaskWithLowestPriority()
        {
            var department = new DepartmentCloud();

            var lowPriorityTask = new Task(1, "Low Priority Task", "Resource 1");
            var highPriorityTask = new Task(2, "High Priority Task", "Resource 2");
            var highestPrioryTask = new Task(5, "Highest Priority Task", "Resource 3");

            department.LogTask(new string[] { lowPriorityTask.Priority.ToString(), lowPriorityTask.Label, lowPriorityTask.ResourceName });
            department.LogTask(new string[] { highPriorityTask.Priority.ToString(), highPriorityTask.Label, highPriorityTask.ResourceName });
            department.LogTask(new string[] { highestPrioryTask.Priority.ToString(), highestPrioryTask.Label, highestPrioryTask.ResourceName });

    
            bool result = department.CreateResource();

            Assert.IsTrue(result);
            Assert.AreEqual(lowPriorityTask.ResourceName, department.Resources.First().Name);
        }

        [Test]
        public void TestResource_MarksResource_AsTested()
        {
            DepartmentCloud department = new DepartmentCloud();
            department.LogTask(new string[] { "5", "Task 2", "Resource 666" });
            department.CreateResource();

            Resource testedResource = department.TestResource("Resource 666");
            bool result = testedResource != null && testedResource.IsTested == true;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Test_That_Resource_ReturnsNull_If_ResourceNotFound()
        {
            DepartmentCloud departmentCloud = new DepartmentCloud();

            Resource testedResource = departmentCloud.TestResource("WhatverResourceString");

            Assert.AreEqual(null, testedResource);
        }
    }
}