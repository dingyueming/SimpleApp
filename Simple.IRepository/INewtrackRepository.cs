using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface INewtrackRepository : IBaseRepository<NewTrackEntity>
    {
        Task<List<NewTrackEntity>> GetNewtracksByDeviceId(dynamic queryModel);

        Task<List<NewTrackEntity>> GetNewTrackEntities(string keyword);
    }
}
