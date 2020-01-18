using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.DM
{
    public class PersonService : IPersonService
    {
        private readonly IDmDomainService dmDomainService;
        public PersonService(IDmDomainService dmDomainService)
        {
            this.dmDomainService = dmDomainService;
        }
        public async Task<bool> Add(PersonExEntity exEntity)
        {
            return await dmDomainService.AddPerson(exEntity);
        }

        public async Task<bool> Delete(List<PersonExEntity> exEntities)
        {
            return await dmDomainService.DeletePerson(exEntities);
        }

        public async Task<bool> Update(PersonExEntity exEntity)
        {
            return await dmDomainService.UpdatePerson(exEntity);
        }

        public async Task<Pagination<PersonExEntity>> GetPage(Pagination<PersonExEntity> param)
        {
            return await dmDomainService.GetPersonPage(param);
        }
    }
}
