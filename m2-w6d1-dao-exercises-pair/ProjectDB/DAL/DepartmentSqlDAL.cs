using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {

        private const string getDepartmentsSql = "select * from department";
        private string connectionString;
        private const string getDepartmentSql = "select* from department order by department_id";
        private string SQL_CreateDepartment = @"insert into department values (@name)";
        private string SQL_UpdateDepartment = @"update department set name = @name where department_id = @department_id";
        //where department_id = @department_id

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
<<<<<<< HEAD
            List<Department> department = new List<Department>();
=======
            List<Department> departments = new List<Department>();
>>>>>>> f352e0e3bc069be361bcacae1573423e0ce7ebdb
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

<<<<<<< HEAD
                    SqlCommand cmd = new SqlCommand(getDepartmentSql, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        department.Add(CreateDepartmentFromRow(results));
                    }
                }
                }
            catch (SqlException)
=======
                    SqlCommand command = new SqlCommand(getDepartmentsSql, conn);
                    SqlDataReader results = command.ExecuteReader();

                    while(results.Read())
                    {
                        departments.Add(CreateDepartmentFromRow(results));
                    }

                }
            }
            catch (SqlException ex)
>>>>>>> f352e0e3bc069be361bcacae1573423e0ce7ebdb
            {
                throw;
            }

<<<<<<< HEAD
            return department;
=======
            return departments;
>>>>>>> f352e0e3bc069be361bcacae1573423e0ce7ebdb
        }

        public bool CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_CreateDepartment, conn);
                    cmd.Parameters.AddWithValue("@department_id", newDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            
        }

        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@department_id", updatedDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            throw new NotImplementedException();
        }

<<<<<<< HEAD
        //HELPER METHOD
=======
>>>>>>> f352e0e3bc069be361bcacae1573423e0ce7ebdb
        private Department CreateDepartmentFromRow(SqlDataReader results)
        {
            Department dept = new Department();
            dept.Id = Convert.ToInt32(results["department_id"]);
            dept.Name = Convert.ToString(results["name"]);
<<<<<<< HEAD

            return dept;
        }
=======
            return dept;
        }

>>>>>>> f352e0e3bc069be361bcacae1573423e0ce7ebdb
    }
}
