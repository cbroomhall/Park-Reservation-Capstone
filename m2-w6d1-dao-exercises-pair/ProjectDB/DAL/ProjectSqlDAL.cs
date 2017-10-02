using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {
        private string connectionString;
        private string getProjectSql = @"Select * from project";
        private string assignEmployeeSql = @"insert into project_employee values (@project_id, @employee_id)";
        private string removeEmployeeSql = @"delete from project_employee where project_id = @project_id and employee_id = @employee_id";
        private string createProjectSql = @"insert into project values (@name, @from_date, @to_date)";

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        
        public List<Project> GetAllProjects()
        {
            List<Project> project = new List<Project>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getProjectSql, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        project.Add(CreateProjectFromRow(results));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return project;
        }
        
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(assignEmployeeSql, conn);
                    cmd.Parameters.AddWithValue("@project_id", projectId);
                    cmd.Parameters.AddWithValue("@employee_id", employeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(removeEmployeeSql, conn);
                    cmd.Parameters.AddWithValue("@project_id", projectId);
                    cmd.Parameters.AddWithValue("@employee_id", employeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public bool CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(createProjectSql, conn);
                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@from_date", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@to_date", newProject.EndDate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }


        //HELPER METHOD
        public Project CreateProjectFromRow(SqlDataReader results)
        {
            Project proj = new Project();
            proj.ProjectId = Convert.ToInt32(results["project_id"]);
            proj.Name = Convert.ToString(results["name"]);
            proj.StartDate = Convert.ToDateTime(results["from_date"]);
            proj.EndDate = Convert.ToDateTime(results["to_date"]);
            
            return proj;

        }

    }
}
