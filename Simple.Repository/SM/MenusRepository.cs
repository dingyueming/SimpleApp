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
    public class MenusRepository : BaseRepository<MenusEntity>, IMenusRepository
    {
        public MenusRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public async Task<int> Add(MenusEntity entity)
        {
            return 0;
        }

        public async Task<List<MenusEntity>> GetAll()
        {
            var result = await base.GetAllAsync();
            return result.ToList();
        }
    }
}
