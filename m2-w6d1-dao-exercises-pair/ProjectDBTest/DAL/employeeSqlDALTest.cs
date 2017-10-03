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
    public class employeeSqlDALTest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Projects;Integrated Security=True";
        private int employeeId;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand("INSERT INTO Department VALUES ('ABC');", conn);
                cmd.ExecuteNonQuery();

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
        public void TestGetAllEmployeeSqlDAL1()
        {
            EmployeeSqlDAL dalEmp = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dalEmp.GetAllEmployees();
            Assert.AreEqual(employeeId, employees[employees.Count - 1].EmployeeId);
           
        }

        [TestMethod]
        public void TestGetEmployeeWithoutProjectsSqlDAL1()
        {
            EmployeeSqlDAL dalEmp = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dalEmp.GetEmployeesWithoutProjects();
            Assert.AreEqual(employeeId, employees[employees.Count - 1].EmployeeId);

        }

        [TestMethod]
        public void TestEmployeeSearchSqlDAL1()
        {
            EmployeeSqlDAL dalEmp = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dalEmp.Search("Harry", "Potter");
            Assert.AreEqual(employeeId, employees[employees.Count - 1].EmployeeId);

        }

    }
}
