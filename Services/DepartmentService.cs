using EmployeeManagement.Models;
using Oracle.ManagedDataAccess.Client;

namespace EmployeeManagement.Services
{
    public class DepartmentService
    {
        public List<Department> GetAll()
        {
            var list = new List<Department>();
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, NAME FROM DEPARTMENTS";
            using var reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                list.Add(new Department
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return list;
        }

        public void Add(Department dept)
        {
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO DEPARTMENTS (ID, NAME) VALUES (:id, :name)";
            cmd.Parameters.Add(new OracleParameter("id", dept.Id));
            cmd.Parameters.Add(new OracleParameter("name", dept.Name));
            cmd.ExecuteNonQuery();
        }

        public void Update(Department dept)
        {
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE DEPARTMENTS SET NAME = :name WHERE ID = :id";
            cmd.Parameters.Add(new OracleParameter("name", dept.Name));
            cmd.Parameters.Add(new OracleParameter("id", dept.Id));
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM DEPARTMENTS WHERE ID = :id";
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }
    }
}
