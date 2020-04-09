using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Data.Common;
using System.Reflection.Emit;
using Dapper.Contrib.Extensions;
using Dapper;

namespace Simple.Infrastructure.Dapper.Contrib
{
    /// <summary>
    /// 数据库访问基类
    /// </summary>
    /// <typeparam name="T">实体类类型</typeparam>
    public partial class BaseRepository<T> where T : class
    {
        /// <summary>
        /// 对象的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键属性对象
        /// </summary>
        public PropertyInfo PrimaryKey { get; set; }

        private IDbConnection _connection;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = ConnectionFactory.CreateConnection();
                    _connection.Open();
                }
                return _connection;
            }
            set => value = _connection;
        }
        public IDbConnection GetDbConnection()
        {
            return Connection;
        }
        /// <summary>
        /// 返回数据库所有的对象集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                return dbConnection.GetAll<T>();
            }
        }

        /// <summary>
        /// 查询数据库,返回指定ID的对象
        /// </summary>
        /// <param name="id">主键的值</param>
        /// <returns></returns>
        public T FindByID(object id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                return dbConnection.Get<T>(id);
            }
        }

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <returns></returns>
        public bool Insert(T info)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Insert(info);
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 插入指定对象集合到数据库中
        /// </summary>
        /// <param name="list">指定的对象集合</param>
        /// <returns></returns>
        public bool Insert(IEnumerable<T> list)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                result = dbConnection.Insert(list) > 0;
            }
            return result;
        }

        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <returns></returns>
        public bool Update(T info)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                result = dbConnection.Update(info);
            }
            return result;
        }
        /// <summary>
        /// 更新指定对象集合到数据库中
        /// </summary>
        /// <param name="list">指定的对象集合</param>
        /// <returns></returns>
        public bool Update(IEnumerable<T> list)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                result = dbConnection.Update(list);
            }
            return result;
        }
        /// <summary>
        /// 从数据库中删除指定对象
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <returns></returns>
        public bool Delete(T info)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                result = dbConnection.Delete(info);
            }
            return result;
        }
        /// <summary>
        /// 从数据库中删除指定对象集合
        /// </summary>
        /// <param name="list">指定的对象集合</param>
        /// <returns></returns>
        public bool Delete(IEnumerable<T> list)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                result = dbConnection.Delete(list);
            }
            return result;
        }
        /// <summary>
        /// 根据指定对象的ID,从数据库中删除指定对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public bool Delete(object id)
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                string query = string.Format("DELETE FROM {0} WHERE {1} = :id", TableName, PrimaryKey.Name);
                var parameters = new DynamicParameters();
                parameters.Add(":id", id);

                result = dbConnection.Execute(query, parameters) > 0;
            }
            return result;
        }
        /// <summary>
        /// 从数据库中删除所有对象
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
            bool result = false;
            using (IDbConnection dbConnection = Connection)
            {
                result = dbConnection.DeleteAll<T>();
            }
            return result;
        }

        /// <summary>
        /// dapper通用分页方法
        /// </summary>
        /// <typeparam name="T">泛型集合实体类</typeparam>
        /// <param name="conn">数据库连接池连接对象</param>
        /// <param name="files">列</param>
        /// <param name="tableName">表</param>
        /// <param name="where">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前页显示条数</param>
        /// <param name="total">结果集总数</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetPageList(string files, string tableName, string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            int skip = 1;
            if (pageIndex > 0)
            {
                skip = (pageIndex - 1) * pageSize + 1;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT COUNT(1) FROM {0} where {1};", tableName, where);
            sb.AppendFormat(@"SELECT  {0}
                                FROM(SELECT ROW_NUMBER() OVER(ORDER BY {3}) AS RowNum,{0}
                                          FROM  {1}
                                          WHERE {2}
                                        ) AS result
                                WHERE  RowNum >= {4}   AND RowNum <= {5}
                                ORDER BY {3}", files, tableName, where, orderby, skip, pageIndex * pageSize);
            using (var reader = Connection.QueryMultiple(sb.ToString()))
            {
                total = reader.ReadFirst<int>();
                return reader.Read<T>();
            }
        }
    }
}
