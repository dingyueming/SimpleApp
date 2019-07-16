using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Entity;
using Simple.IRepository.SM;

namespace Simple.Repository.SM
{
    public class AuthRepository : IAuthRepository
    {
        public async void Add(AUTHEntity entity)
        {
            var dbContext = DbContextFactory.Default;
            var aa = dbContext.Auth.Insert(entity);
            throw new NotImplementedException();
        }
    }
}
