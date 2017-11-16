using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using ProjectDB.Models;
using System.Transactions;
using System.Data.SqlClient;
using ProjectDB.DAL;


namespace ProjectDBTest
{
    [TestClass]
    public class departmentSQLDALTest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Projects;Integrated Security=True";
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
            }
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }

        [TestMethod]
        public void TestGetDepartment()
        {
            DepartmentSqlDAL deptDAL = new DepartmentSqlDAL(connectionString);
            List<Department> depts = deptDAL.GetDepartments();
            Assert.AreEqual(departmentId, depts[depts.Count - 1].Id); 
            
        }

        [TestMethod]
        public void TestUpdateDepartment()
        {
            DepartmentSqlDAL deptDAL = new DepartmentSqlDAL(connectionString);
            Department updatedDepartment = new Department()
            {
                Id = departmentId,
                Name = "XYZ"
            };
            bool result = deptDAL.UpdateDepartment(updatedDepartment);
            List<Department> depts = deptDAL.GetDepartments();
            Assert.AreEqual("XYZ", depts[depts.Count - 1].Name);
            Assert.AreEqual(true, result);
        }
    }
}
