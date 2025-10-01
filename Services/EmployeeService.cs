using EmployeeManagement.Models;
using Oracle.ManagedDataAccess.Client;

namespace EmployeeManagement.Services
{
    public class EmployeeService
    {
        public List<Employee> GetAll()
        {
            var list = new List<Employee>();
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, NAME, EMAIL, DEPARTMENT_ID FROM EMPLOYEES";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Employee
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    DepartmentId = reader.GetInt32(3)
                });
            }
            return list;
        }

        public void Add(Employee emp)
        {
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO EMPLOYEES (ID, NAME, EMAIL, DEPARTMENT_ID) VALUES (:id, :name, :email, :dept)";
            cmd.Parameters.Add(new OracleParameter("id", emp.Id));
            cmd.Parameters.Add(new OracleParameter("name", emp.Name));
            cmd.Parameters.Add(new OracleParameter("email", emp.Email));
            cmd.Parameters.Add(new OracleParameter("dept", emp.DepartmentId));
            cmd.ExecuteNonQuery();
        }

        public void Update(Employee emp)
        {
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE EMPLOYEES SET NAME = :name, EMAIL = :email, DEPARTMENT_ID = :dept WHERE ID = :id";
            cmd.Parameters.Add(new OracleParameter("name", emp.Name));
            cmd.Parameters.Add(new OracleParameter("email", emp.Email));
            cmd.Parameters.Add(new OracleParameter("dept", emp.DepartmentId));
            cmd.Parameters.Add(new OracleParameter("id", emp.Id));
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = OracleDbService.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM EMPLOYEES WHERE ID = :id";
            cmd.Parameters.Add(new OracleParameter("id", id));
            cmd.ExecuteNonQuery();
        }
    }
}
