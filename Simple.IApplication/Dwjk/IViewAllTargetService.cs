using Simple.ExEntity.Map;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.Dwjk
{
    public interface IViewAllTargetService
    {
        Task<ViewAllTargetExEntity> GetViewAllTarget(string keyword);

        Task<List<ViewAllTargetExEntity>> GetViewAllTargetListByOrgcode(string code);
    }
}
