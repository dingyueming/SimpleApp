using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Simple.Entity;

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
        public IConfiguration _configuration;
        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public DbContext GetDb()
        {
            var connString = _configuration["ConnectionStr"];
            var connection = new OracleConnection(connString);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            DbContext dbContext = DbContext.Init(connection, 5);
            return dbContext;
        }

        public DbContext Default => this.GetDb();
    }
}
