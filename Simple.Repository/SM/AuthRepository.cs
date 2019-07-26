using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Entity;
using Simple.Infrastructure;
using Simple.IRepository.SM;

namespace Simple.Repository.SM
{
    public class AuthRepository : IAuthRepository
    {
        private DbContextFactory _dbContextFactory { get; set; }
        public AuthRepository(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async void Add(AUTHEntity entity)
        {
            var dbContext = _dbContextFactory.Default;
            var aa = dbContext.Auth.Insert(entity);
            throw new NotImplementedException();
        }
    }
}
