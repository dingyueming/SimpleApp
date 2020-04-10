using Simple.ExEntity.Map;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.MapShow
{
    public interface ILastLocatedService
    {
        Task<LastLocatedExEntity> GetLastLocatedByMac(string mac);
    }
}
