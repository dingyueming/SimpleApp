using System;
using Simple.IApplication.SystemModule;
using Simple.IDomain;

namespace Simple.Application.SM
{
    public class UserService : IUserService
    {
        private readonly ISmDomainService _smDomainService;
        public UserService(ISmDomainService smDomainService)
        {
            _smDomainService = smDomainService;
        }
        public void Add()
        {
            _smDomainService.AddUser();
            throw new NotImplementedException();
        }
    }
}
