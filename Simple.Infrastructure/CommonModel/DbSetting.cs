using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;

namespace Simple.Infrastructure.CommonModel
{
    public class DbSetting
    {

        public DbSetting(string dbType, string connectionStr)
        {
            DbType = dbType;
            ConnectionStr = connectionStr;
        }
        public string DbType { get; private set; } = "mysql";

        public string ConnectionStr { get; private set; } = "server=rm-wz90n4nhkp75xo28o8o.mysql.rds.aliyuncs.com;database=simpledb;userid=root;password=Admin123";

        public DbConnection DbConnection
        {
            get
            {
                DbConnection dbConnection = null;
                switch (DbType)
                {
                    case "mysql":
                        dbConnection = new MySqlConnection(ConnectionStr);
                        break;
                    default:
                        dbConnection = new MySqlConnection(ConnectionStr);
                        break;
                }
                return dbConnection;
            }
        }
    }
}
