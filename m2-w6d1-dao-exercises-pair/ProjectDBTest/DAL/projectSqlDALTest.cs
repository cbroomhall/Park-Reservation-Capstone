using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using ProjectDB.Models;
using System.Transactions;
using System.Data.SqlClient;
using ProjectDB.DAL;

namespace ProjectDBTest.DAL
{
    [TestClass]
    public class projectSqlDALTest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Projects;Integrated Security=True";
        private int projectId;
        private int employeeId;
        private int departmentId;
         

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand("INSERT INTO Department VALUES ('ABC'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                departmentId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO Project VALUES ('ABC', '2010-01-01', '2017-09-28'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                projectId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO Employee VALUES ((SELECT department_id from department where name = 'ABC'), 'Harry', 'Potter', 'Wizard', '1980-01-01', 'M', '2017-01-01'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                employeeId = (int)cmd.ExecuteScalar();
                

            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }

        [TestMethod]
        public void TestGetAllProjects()
        {
            ProjectSqlDAL projectDal = new ProjectSqlDAL(connectionString);
            List<Project> projectList = projectDal.GetAllProjects();
            Assert.AreEqual(projectId, projectList[projectList.Count - 1].ProjectId);
        }

        [TestMethod]
        public void TestAssingnEmpToProject()
        {
            ProjectSqlDAL projectDal = new ProjectSqlDAL(connectionString);
            bool result = projectDal.AssignEmployeeToProject(projectId, employeeId);
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void TestRemoveEmpFromProject()
        {
            ProjectSqlDAL projectDal = new ProjectSqlDAL(connectionString);
            bool result = projectDal.AssignEmployeeToProject(projectId, employeeId);
            Assert.AreEqual(true, result);
            result = projectDal.RemoveEmployeeFromProject(projectId, employeeId);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestCreateProject()
        {
            ProjectSqlDAL projectDal = new ProjectSqlDAL(connectionString);
            Project ourProject = new Project()
            {
                ProjectId = projectId + 1,
                Name = "XYZ",
                StartDate = new DateTime(2011, 1, 1),
                EndDate = new DateTime(2011, 12, 1)
            };
            bool result = projectDal.CreateProject(ourProject);
            List<Project> projects = projectDal.GetAllProjects();
            Assert.AreEqual("XYZ", projects[projects.Count - 1].Name);
            Assert.AreEqual(true, result);
        }
    }
}
