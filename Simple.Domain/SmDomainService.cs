using System;
using System.Collections.Generic;
using System.Text;
using Simple.Entity;
using Simple.IDomain;
using Simple.IRepository;
using Simple.IRepository.SM;

namespace Simple.Domain
{
    public class SmDomainService : ISmDomainService
    {
        private readonly IAuthRepository _authRepository;
        public SmDomainService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public void AddUser()
        {
            _authRepository.Add(new AUTHEntity());
        }
    }
}
