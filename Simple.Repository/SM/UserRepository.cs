using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure;
using Simple.IRepository.SM;

namespace Simple.Repository.SM
{
    public class UsersRepository : IUserRepository
    {
        private DbContextFactory _dbContextFactory { get; set; }
        public UsersRepository(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<int> Add(UsersEntity entity)
        {
            var dbContext = _dbContextFactory.Default;
            var id = await dbContext.Users.InsertAsync(entity);
            return id ?? 0;
        }

        public async Task<List<UsersEntity>> GetAll()
        {
            var dbContext = _dbContextFactory.Default;
            var list = await dbContext.Users.AllAsync();
            return list.ToList();
        }
    }
}
