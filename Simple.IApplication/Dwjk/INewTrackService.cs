using Simple.ExEntity.Map;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.Dwjk
{
    public interface INewTrackService
    {
        Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword, DateTime startTime, DateTime endTime);
    }
}
