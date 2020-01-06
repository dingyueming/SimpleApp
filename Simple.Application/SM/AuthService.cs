using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.ExEntity;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.IDomain;

namespace Simple.Application.SM
{
    public class AuthService : IAuthService
    {
        private readonly ISmDomainService _smDomainService;
        public AuthService(ISmDomainService smDomainService)
        {
            _smDomainService = smDomainService;
        }
        public void Add()
        {
            throw new NotImplementedException();
        }

        public async Task<List<AuthExEntity>> GetAllUsers()
        {
            return null;
        }
    }
}
