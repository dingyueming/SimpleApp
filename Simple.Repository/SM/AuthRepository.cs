using System;
using System.Collections.Generic;
using System.Text;
using Simple.Entity;
using Simple.IRepository.SM;

namespace Simple.Repository.SM
{
    public class AuthRepository : IAuthRepository
    {
        public void Add(AUTHEntity entity)
        {
            var dbContext = DbContextFactory.Default;
            var aa = dbContext.AuthTable.Get(1);
            throw new NotImplementedException();
        }
    }
}
