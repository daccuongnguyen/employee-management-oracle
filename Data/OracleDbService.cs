using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace EmployeeManagement.Data;

public class OracleDbService
{
    private readonly string _conn;
    public OracleDbService(IConfiguration config)
    {
        _conn = config.GetSection("Oracle").GetValue<string>("ConnectionString") ?? throw new Exception("Connection string missing");
    }

    public IDbConnection GetConnection()
    {
        return new OracleConnection(_conn);
    }

    // Helper to get next sequence value (expects sequence name)
    public long NextSeq(string seqName)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {seqName}.NEXTVAL FROM DUAL";
        var val = cmd.ExecuteScalar();
        return Convert.ToInt64(val);
    }
}
