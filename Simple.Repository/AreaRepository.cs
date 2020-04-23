using Dapper;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Repository
{
    public class AreaRepository : BaseRepository<AreaEntity>, IAreaRepository
    {
        public async Task<Pagination<AreaEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<AreaEntity>();
            string totalSql = $"select count(1) from area a where 1=1 ";
            var sql = "select a.*,b.* from area a left join area_detail b on a.areaid=b.areaid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            List<AreaEntity> areaList = new List<AreaEntity>();
            var list = await Connection.QueryAsync<AreaEntity, AreaDetailEntity, AreaEntity>(sql, (a, b) =>
            {
                var currentArea = areaList.FirstOrDefault(x => x.AREAID == a.AREAID);
                if (currentArea == null)
                {
                    a.AreaDetails.Add(b);
                    areaList.Add(a);
                }
                else
                {
                    currentArea.AreaDetails.Add(b);
                }
                return a;
            }, splitOn: "AREAID");
            sql = $"select a.*,b.*,c.* from area a left join car_area b on a.areaid=b.areaid left join view_all_target c on b.carid=c.carid where a.areaid in ({string.Join(',', areaList.Select(x => x.AREAID).ToArray())})";

            var list2 = await Connection.QueryAsync<AreaEntity, CarAreaEntity, ViewAllTargetEntity, AreaEntity>(sql, (a, b, c) =>
             {
                 var currentArea = areaList.Find(x => x.AREAID == a.AREAID);
                 if (currentArea != null && b != null)
                 {
                     currentArea.CarAreas.Add(b);
                 }
                 if (currentArea != null && c != null)
                 {
                     currentArea.Devices.Add(c);
                 }
                 return a;
             }, splitOn: "CARID,CARID");

            pagination.Data = areaList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public async Task<AreaEntity> GetEntityForValdata(AreaEntity entity)
        {
            var sql = "select * from area a where a.areaname = :areaname";
            return await Connection.QueryFirstOrDefaultAsync<AreaEntity>(sql, new { areaname = entity.AREANAME });
        }

        public async Task InsertAlarmArea(AreaEntity entity)
        {
            var insertAreaSql = @"insert into area
                                  (areaid, areaname, unitid, centerid, areatype, userid, overspeed, workarea, isshare, oil_rect_type)
                                values
                                  (:areaid, :areaname, :unitid, :centerid, :areatype, :userid, :overspeed, :workarea, :isshare, :oil_rect_type)";
            var insertAreaDetailSql = @"insert into area_detail
                                      (areaid, pointno, longtitude, latitude)
                                    values
                                      (:areaid, :pointno, :longtitude, :latitude)";
            var trans = Connection.BeginTransaction();
            try
            {
                var areaId = await Connection.ExecuteScalarAsync<int>("select area_sq.nextval from dual");
                entity.AREAID = areaId;
                entity.AreaDetails.ForEach((x) =>
                {
                    x.AREAID = areaId;
                });
                await Connection.ExecuteAsync(insertAreaSql, entity);
                await Connection.ExecuteAsync(insertAreaDetailSql, entity.AreaDetails);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public async Task<AreaEntity> GetAreaEntityById(int areaId)
        {
            var sql = $"select a.*,b.* from area a left join area_detail b on a.areaid=b.areaid where a.areaid={areaId} ";
            AreaEntity areaEntity = null;
            await Connection.QueryAsync<AreaEntity, AreaDetailEntity, AreaEntity>(sql, (a, b) =>
            {
                if (areaEntity == null)
                {
                    areaEntity = a;
                    areaEntity.AreaDetails.Add(b);
                }
                else
                {
                    areaEntity.AreaDetails.Add(b);
                }
                return a;
            }, splitOn: "AREAID");
            return areaEntity;
        }

    }
}
