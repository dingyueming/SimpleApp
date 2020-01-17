using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Simple.Entity;
using Simple.ExEntity;
using Simple.ExEntity.SM;
using System.Linq;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.Domain
{
    /// <summary>
    /// automapper映射配置
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            SetMapperInstence();
        }
        /// <summary>
        /// 实体和扩展实体映射初始化
        /// </summary>
        public void SetMapperInstence()
        {
            var entityAassembly = System.Reflection.Assembly.Load("Simple.Entity");
            var exEntityAassembly = System.Reflection.Assembly.Load("Simple.ExEntity");
            var entityTypes = entityAassembly.GetTypes();
            var exEntityTypes = exEntityAassembly.GetTypes();
            foreach (var entityType in entityTypes)
            {
                var exTypeName = entityType.Name.Replace("Entity", "") + "ExEntity";
                var listExEntityType = exEntityTypes.Where(x => x.Name == exTypeName).ToList();
                foreach (var exEntityType in listExEntityType)
                {
                    CreateMap(entityType, exEntityType);
                    CreateMap(exEntityType, entityType);
                }
            }
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}
