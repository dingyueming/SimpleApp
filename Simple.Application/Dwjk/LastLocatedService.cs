using Simple.ExEntity.Map;
using Simple.IApplication.Dwjk;
using Simple.IDomain;
using System.Threading.Tasks;

namespace Simple.Application.Dwjk
{
    public class LastLocatedService : ILastLocatedService
    {
        private readonly IIfDomainService ifDomainService;
        public LastLocatedService(IIfDomainService ifDomainService)
        {
            this.ifDomainService = ifDomainService;
        }

        public async Task<LastLocatedExEntity> GetLastLocatedByMac(string mac)
        {
            return await ifDomainService.GetLastLocatedByMac(mac);
        }
    }
}
