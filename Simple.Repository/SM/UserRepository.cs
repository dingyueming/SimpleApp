using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository.SM;

namespace Simple.Repository.SM
{
    public class UsersRepository : BaseRepository<UsersEntity>, IUserRepository
    {
        public UsersRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
            //base.PrimaryKey=
        }



        public async Task<int> Add(UsersEntity entity)
        {
            var id = await base.InsertAsync(entity);
            return 0;
        }

        public async Task<List<UsersEntity>> GetAll()
        {
            var result = await base.GetAllAsync();
            return result.ToList();
        }
    }
}
