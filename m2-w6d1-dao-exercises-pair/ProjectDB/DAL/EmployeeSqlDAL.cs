using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private string connectionString;
        private const string getEmployeeSql = @"Select * from employee";
        private string getSpecificEmployeeSql = @"Select * from employee";
        private string getProjectlessEmployees = @"select * from employee where employee_id not in (select employee_id from project_employee)";

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employee = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getEmployeeSql, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        employee.Add(CreateEmployeeFromRow(results));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return employee;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> employee = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getSpecificEmployeeSql, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        Employee emply = CreateEmployeeFromRow(results);
                        if (emply.FirstName.Equals(firstname) && emply.LastName.Equals(lastname))
                        {
                            employee.Add(emply);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return employee;
        }

        public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employee = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getProjectlessEmployees, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        employee.Add(CreateEmployeeFromRow(results));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return employee;
        }


        //HELPER METHOD
        public Employee CreateEmployeeFromRow(SqlDataReader results)
        {
            Employee emply = new Employee();
            emply.EmployeeId = Convert.ToInt32(results["employee_id"]);
            emply.DepartmentId = Convert.ToInt32(results["department_id"]);
            emply.FirstName = Convert.ToString(results["first_name"]);
            emply.LastName = Convert.ToString(results["last_name"]);
            emply.JobTitle = Convert.ToString(results["job_title"]);
            emply.HireDate = Convert.ToDateTime(results["hire_date"]);
            emply.BirthDate = Convert.ToDateTime(results["birth_date"]);
            emply.Gender = Convert.ToString(results["gender"]);

            return emply;

        }
    }
}
