using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Simple.Entity;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System.Linq;

namespace Simple.Repository
{
    public class InterfaceRepository : BaseRepository<InterfaceEntity>, IInterfaceRepository
    {
        public async Task<Pagination<InterfaceEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<InterfaceEntity>();
            var list = new List<InterfaceEntity>();
            var totalSql = $"select count(1) from tbl_interface a where 1=1 ";
            var sql = "select a.*,b.*,c.* from tbl_interface a left join tbl_interface_right b on a.app_id=b.app_id left join unit c on c.org_code=b.org_code where 1=1";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            await Connection.QueryAsync<InterfaceEntity, InterfaceRightEntity, UnitEntity, InterfaceEntity>(sql, (a, b, c) =>
              {
                  var interfaceEntity = list.FirstOrDefault(x => x.App_Id == a.App_Id);
                  if (interfaceEntity == null)
                  {
                      if (b != null)
                      {
                          b.Unit = c;
                          a.Right.Add(b);
                      }
                      list.Add(a);
                  }
                  else
                  {
                      if (b != null)
                      {
                          b.Unit = c;
                          interfaceEntity.Right.Add(b);
                      }
                  }
                  return a;
              }, splitOn: "app_id,UnitId");
            pagination.Data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public override async Task<bool> InsertAsync(InterfaceEntity entity)
        {
            var trans = Connection.BeginTransaction();
            try
            {
                #region 数据校验
                var count = await Connection.ExecuteScalarAsync<int>("select count(1) from tbl_interface t where t.app_name=:app_name", new { entity.App_Name }, trans);
                if (count > 0)
                {
                    throw new Exception("应用名称重复！");
                }
                #endregion
                var sql = @"insert into tbl_interface
                             (app_id,app_name, password, ip, port, int_type, ter_type,  status, run_status, proto_type)
                        values
                             (:app_id, :app_name, :password, :ip, :port, :int_type, :ter_type,  :status, :run_status, :proto_type)";

                var sql2 = @"insert into tbl_interface_right (app_id, org_code) values (:app_id, :org_code)";

                await Connection.ExecuteScalarAsync<int>(sql, entity, trans);
                var appId = await Connection.ExecuteScalarAsync<int>("select app_id from tbl_interface t where t.app_name=:app_name", new { entity.App_Name }, trans);
                entity.Right.ForEach(x =>
                {
                    x.App_Id = appId;
                });
                await Connection.ExecuteAsync(sql2, entity.Right, trans);
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public override async Task<bool> UpdateAsync(InterfaceEntity entity)
        {
            var trans = Connection.BeginTransaction();
            try
            {
                #region 数据校验
                var count = await Connection.ExecuteScalarAsync<int>("select count(1) from tbl_interface t where t.app_name=:app_name", new { entity.App_Name }, trans);
                if (count > 0)
                {
                    throw new Exception("应用名称重复！");
                }
                #endregion
                await UpdateAsync(entity, trans);
                var sql = "delete from tbl_interface_right t where t.app_id=:appid";
                Connection.Execute(sql, new { appid = entity.App_Id }, trans);
                var sql2 = @"insert into tbl_interface_right (app_id, org_code) values (:app_id, :org_code)";
                entity.Right.ForEach(x =>
                {
                    x.App_Id = entity.App_Id;
                });
                await Connection.ExecuteAsync(sql2, entity.Right, trans);
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
    }
}
