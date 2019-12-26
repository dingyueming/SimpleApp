using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Simple.Infrastructure.Dapper.Contrib
{
    /// <summary>
    /// 数据库连接辅助类
    /// </summary>
    public class ConnectionFactory
    {

        private IConfiguration configuration;
        public ConnectionFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="databaseType">数据库类型</param>
        /// <returns></returns>
        private static DatabaseType GetDataBaseType(string databaseType)
        {
            DatabaseType returnValue = DatabaseType.SqlServer;
            foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
            {
                if (dbType.ToString().Equals(databaseType, StringComparison.OrdinalIgnoreCase))
                {
                    returnValue = dbType;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            IDbConnection connection = null;            
            //获取配置进行转换
            var type = configuration["DbSetting:DbType"];
            var dbType = GetDataBaseType(type);
            var strConn = configuration["DbSetting:ConnectionStr"];

            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(strConn);
                    break;
                case DatabaseType.MySql:
                    connection = new MySql.Data.MySqlClient.MySqlConnection(strConn);
                    break;
                case DatabaseType.Npgsql:
                    //connection = new Npgsql.NpgsqlConnection(strConn);
                    break;
                case DatabaseType.Sqlite:
                    //connection = new SQLiteConnection(strConn);
                    break;
                case DatabaseType.Oracle:
                    connection = new Oracle.ManagedDataAccess.Client.OracleConnection(strConn);
                    //connection = new System.Data.OracleClient.OracleConnection(strConn);
                    break;
                case DatabaseType.DB2:
                    //connection = new System.Data.OleDb.OleDbConnection(strConn);
                    break;
            }

            return connection;
        }
    }
    /// <summary>
    /// 数据库类型定义
    /// </summary>
    public enum DatabaseType
    {
        SqlServer,  //SQLServer数据库
        MySql,      //Mysql数据库
        Npgsql,     //PostgreSQL数据库
        Oracle,     //Oracle数据库
        Sqlite,     //SQLite数据库
        DB2         //IBM DB2数据库
    }
}
