using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.DM
{
    public interface IPersonService
    {
        Task<bool> Add(PersonExEntity exEntity);
        Task<bool> Delete(List<PersonExEntity> exEntities);
        Task<bool> Update(PersonExEntity exEntity);
        Task<Pagination<PersonExEntity>> GetPage(Pagination<PersonExEntity> param);
    }
}
