using EmployeeManagement;
using Oracle.ManagedDataAccess.Client;

namespace EmployeeManagement.Services
{
    public static class OracleDbService
    {
        private static readonly string connectionString =
            "User Id=approval_c;Password=secret;Data Source=localhost:1521/xe";

        public static OracleConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }
    }
}
