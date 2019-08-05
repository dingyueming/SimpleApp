using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Simple.Entity;
using Simple.ExEntity;

namespace Simple.Domain
{
    /// <summary>
    /// automapper映射配置
    /// </summary>
    public class SimpleProfile : Profile
    {
        public SimpleProfile()
        {
            CreateMap<UsersEntity, UsersExEntity>();
        }
    }
}
