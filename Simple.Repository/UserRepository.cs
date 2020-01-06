using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;

namespace Simple.Repository
{
    public class UsersRepository : BaseRepository<UsersEntity>, IUserRepository
    {
    }
}
