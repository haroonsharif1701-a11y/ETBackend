using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Data
{
    public class Db
    {
        private readonly string _connectionString;

        public Db(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // Create and return open connection
        public SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        // Create command (your pattern usage)
        public SqlCommand GetCommand(string spName)
        {
            var conn = GetConnection();
            var cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }
    }
}