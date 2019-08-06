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
    public class MenusRepository : IMenusRepository
    {
        private DbContextFactory dbContextFactory { get; set; }
        public MenusRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public async Task<int> Add(MenusEntity entity)
        {
            var dbContext = dbContextFactory.Default;
            var id = await dbContext.Users.InsertAsync(entity);
            return id ?? 0;
        }

        public async Task<List<MenusEntity>> GetAll()
        {
            var dbContext = dbContextFactory.Default;
            var list = await dbContext.Menus.AllAsync();
            return list.ToList();
        }
    }
}
