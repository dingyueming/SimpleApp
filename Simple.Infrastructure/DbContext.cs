using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Simple.Entity;
using Newtonsoft.Json;
using Simple.Infrastructure.CommonModel;

namespace Simple.Infrastructure
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public class DbContext : Database<DbContext>
    {
        public Table<UsersEntity> Users { get; set; }
    }

    public class DbContextFactory
    {
        public IConfiguration configuration;
        public DbContextFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public DbContext GetDb()
        {
            var connectionStr = configuration["DbSetting:ConnectionStr"];
            var dbType = configuration["DbSetting:DbType"];
            var dbSettingModel = new DbSetting(dbType, connectionStr);
            var connection = dbSettingModel.DbConnection;
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            DbContext dbContext = DbContext.Init(connection, 5);
            return dbContext;
        }

        public DbContext Default => this.GetDb();
    }
}
