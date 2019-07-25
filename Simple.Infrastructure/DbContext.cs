using Dapper;
using Oracle.ManagedDataAccess.Client;
using Simple.Entity;
using Simple.Infrastructure.Tools;

namespace Simple.Infrastructure
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public class DbContext : Database<DbContext>
    {
        public Table<AUTHEntity> Auth { get; set; }
    }

    public class DbContextFactory
    {
        public DbContextFactory()
        {
            //_connStr = ConfigTool.AppSetting<string>("ConnectionStr");
        }


        public static DbContext GetDb(string connString)
        {
            var connection = new OracleConnection(connString);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            DbContext dbContext = DbContext.Init(connection, 5);
            return dbContext;
        }

        public static DbContext Default => GetDb("User ID=newgps;Password=newgps;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.168.1.12)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORAGPS)))");
    }
}
