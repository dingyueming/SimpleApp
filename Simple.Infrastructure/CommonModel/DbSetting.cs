using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;

namespace Simple.Infrastructure.CommonModel
{
    public class DbSetting
    {
        public string DbType { get; set; } = "mysql";

        public string ConnectionStr { get; set; } = "server=rm-wz90n4nhkp75xo28o8o.mysql.rds.aliyuncs.com;database=simpledb;userid=root;password=Admin123";

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
