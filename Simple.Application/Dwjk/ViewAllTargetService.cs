using Simple.ExEntity.Map;
using Simple.IApplication.Dwjk;
using Simple.IDomain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Application.Dwjk
{
    public class ViewAllTargetService : IViewAllTargetService
    {
        private readonly IIfDomainService ifDomainService;
        public ViewAllTargetService(IIfDomainService ifDomainService)
        {
            this.ifDomainService = ifDomainService;
        }

        public async Task<ViewAllTargetExEntity> GetViewAllTarget(string keyword)
        {
            return await ifDomainService.GetViewAllTarget(keyword);
        }

        public async Task<List<ViewAllTargetExEntity>> GetViewAllTargetListByOrgcode(string code)
        {
            return await ifDomainService.GetViewAllTargetListByOrgcode(code);
        }
    }
}
