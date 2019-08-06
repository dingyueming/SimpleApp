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
        #region SM模块
        /// <summary>
        /// 用户表
        /// </summary>
        public Table<UsersEntity> Users { get; set; }
        /// <summary>
        /// 菜单表
        /// </summary>
        public Table<MenusEntity> Menus { get; set; }
        #endregion
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
