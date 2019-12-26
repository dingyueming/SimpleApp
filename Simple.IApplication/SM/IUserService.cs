using Simple.ExEntity;
using Simple.ExEntity.SM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SM
{
    public interface IAuthService
    {
        void Add();

        Task<List<AuthExEntity>> GetAllUsers();
    }
}
