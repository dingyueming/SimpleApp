using System.Data.SqlClient;
using Dapper;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

namespace Simple.Repository
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public class DbContext : Database<DbContext>
    {

    }

    public class DbContextFactory
    {
        public static DbContext GetDb(string connString)
        {
            var connection = new OracleConnection(connString);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            DbContext dbContext = DbContext.Init(connection, 5);
            return dbContext;
        }

        public static DbContext Default => GetDb("");
    }
}
